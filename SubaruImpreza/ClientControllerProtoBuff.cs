using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using Google.Protobuf;
using Moto.Protocol;
using IResponse = NetworkingModule.common.IResponse;


namespace SubaruImpreza
{
    public class ClientControllerProtoBuff
    {
        private Form form;
        private Form formLogin;
        private volatile bool finished;
        private TcpClient connection;
        private NetworkStream stream;

        public ClientControllerProtoBuff(Form form1, Form formLogin)
        {
            this.form = form1;
            this.formLogin = formLogin;
            startReader();
        }

        private void initializeConnection()
        {
            try
            {
                connection = new TcpClient("127.0.0.1", 9999);
                finished = false;
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void startReader()
        {
            Thread tw = new Thread(run);

            tw.Start();
        }

        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    initializeConnection();
                    stream = connection.GetStream();
                    Moto.Protocol.IResponse response = 
                        Moto.Protocol.IResponse.Parser.ParseDelimitedFrom(stream);
                    Console.WriteLine("response received " + response);
                    if (response.ResponseDto != null)
                    {
                        Console.WriteLine("Responded Participant from server");
                        handleResponseParticipantCliControl(response.ResponseDto);
                    }
                    else if (response.UserDto != null)
                    {
                        Console.WriteLine("Responded User List from server");
                        handleResponseUserCliControl(response.UserDto);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }

        private void sendRequest(IRequest request)
        {
            try
            {
                //formatter.Serialize(stream, request);
                if (connection == null)
                {
                    initializeConnection();
                }
                stream = connection.GetStream();
                request.WriteDelimitedTo(stream);
              //  stream.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object " + e);
            }
        }

        private void handleResponseParticipantCliControl(ResponseDto response)
        {
            string type = response.ResponseType;
            string message = response.Message;
            if (message.Equals("Success"))
            {
                NetworkingModule.common.ResponseDto dto = new NetworkingModule.common.ResponseDto();
                dto.ResponseType = response.ResponseType;
                dto.Message = response.Message;
                List<DomainModule.domain.Participant> participants = new List<DomainModule.domain.Participant>();
                foreach (var VARIABLE in response.Participants)
                {
                    DomainModule.domain.Participant p = new DomainModule.domain.Participant(
                        VARIABLE.Id, 
                        VARIABLE.TeamName, 
                        VARIABLE.Name, 
                        VARIABLE.IdRace
                        );
                    participants.Add(p);
                }
                dto.Participants = participants;
                ParticipantSubject participantSubject = new ParticipantSubject(dto);
                //add ui controller
                participantSubject.addObserver((ResponseObserver)form);
                participantSubject.notifyObserver();
            }
        }

        private void handleResponseUserCliControl(UserDto response)
        {
            string message = response.Message;
            if (message.Equals("Success"))
            {
                NetworkingModule.common.UserDto dto = new NetworkingModule.common.UserDto();
                dto.Message = response.Message;
                HashSet<string> hashSet = new HashSet<string>();
                foreach (var VARIABLE in response.Username)
                {
                    hashSet.Add(VARIABLE);
                }
                dto.Username = hashSet;
                UserSubject userSubject = new UserSubject(dto);
                //add ui controller
                userSubject.addObserver((ResponseObserver)formLogin);
                userSubject.notifyObserver();
            }
        }

        public virtual void addParticipant(DomainModule.domain.Participant participant)
        {
            RequestDto requestDto = new RequestDto();
            requestDto.RequestType = "addParticipant";

            Moto.Protocol.Participant participantProto = new Participant();
            participantProto.Id = participant.Id;
            participantProto.Name = participant.Name;
            participantProto.TeamName = participant.TeamName;
            participantProto.IdRace = participant.IdRace;
            requestDto.Dto = participantProto;
            IRequest irequest = new IRequest();
            irequest.RequestDto = requestDto;
            sendRequest(irequest);
        }

        public virtual void addUser(NetworkingModule.common.UserDto user)
        {
            Moto.Protocol.IRequest request = new Moto.Protocol.IRequest();
            Moto.Protocol.UserDto userDtoProto = new Moto.Protocol.UserDto();
            userDtoProto.Message = user.Message;
            foreach (var VARIABLE in user.Username)
            {
                userDtoProto.Username.Add(VARIABLE);
            }
            request.UserDto = userDtoProto;
            sendRequest(request);
        }

        public virtual void logoutUser(NetworkingModule.common.UserDto user)
        {
            Moto.Protocol.IRequest request = new Moto.Protocol.IRequest();
            Moto.Protocol.UserDto userDtoProto = new Moto.Protocol.UserDto();
            userDtoProto.Message = user.Message;
            foreach (var VARIABLE in user.Username)
            {
                userDtoProto.Username.Add(VARIABLE);
            }
            request.UserDto = userDtoProto;
            sendRequest(request);
            
        }
    }
}