using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using DomainModule.domain;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using ServiceModule.Service;

namespace NetworkingModule.common
{
    public class ClientRpcWorker
    {
        private ParticipantService service;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        private static readonly HashSet<string> activeUsernames = new HashSet<string>();

        public ClientRpcWorker(ParticipantService service, TcpClient connection)
        {
            this.service = service;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object response = handleRequest((IRequest)request);
                    if (response != null)
                    {
                        sendResponse((IResponse)response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private IResponse handleRequest(IRequest request)
        {
            IResponse response = null;
            if (request is RequestDto)
            {
                Console.WriteLine("Participant request");

                Participant participant = ((RequestDto)request).Dto;
                Console.WriteLine("handleRequest Participant ClientRpcWorker -- " + participant);
                try
                {
                    lock (service)
                    {
                        service.add(participant);
                    }

                    Console.WriteLine("Raspuns OkResponse pentru InscrieParticipantRequest ---- ClientRpcWorker");

                    var list = new List<Participant>();
                    list.Add(participant);
                    return new ResponseDto("Participant", "Success", list);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else if (request is UserDto)
            {
                lock (activeUsernames)
                {
                Console.WriteLine("User request");
                UserDto userDto = (UserDto)request;
                string type = userDto.Message;
                HashSet<String> users = userDto.Username;
                if (type.StartsWith("addUser"))
                {
                    HashSet<string> actives = new HashSet<string>();
                    String usrn = userDto.Username.ElementAt(0).Substring(5);
                    Console.WriteLine("Login: " + usrn);

                    activeUsernames.Add(usrn);

                    foreach (var VARIABLE in activeUsernames)
                    {
                        Console.WriteLine(VARIABLE);
                        actives.Add(VARIABLE);
                    }

                    return new UserDto(actives, "Success");
                }
                else if (type.StartsWith("Logout"))
                {
                    String usrn = userDto.Username.ElementAt(0);
                    Console.WriteLine("Logout: "+usrn);
                    activeUsernames.Remove(usrn);
                    }
                }
            }

            return response;
        }

        private void sendResponse(IResponse response)
        {
            Console.WriteLine("sending response crw140" + response);

            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
        }
    }
}