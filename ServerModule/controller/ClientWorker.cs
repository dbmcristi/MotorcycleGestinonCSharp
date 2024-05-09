using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using DomainModule.domain;
using NetworkingModule.common;
using RepoModule.Repo;
using ServiceModule.Service;

namespace ServerModule.controller
{
    public class ClientWorker
    {
        private TcpClient connection;
        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;
        private RaceRepo raceRepo;
        private ParticipantRepo participantRepo; 
        private ParticipantService participantservice ;
        public ClientWorker(TcpClient connection)
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString","Data Source=D:\\\\FACULTATE_23-24\\\\mpp23-24\\\\gitClones\\\\mpp-proiect-java-dbmcristi\\\\motorcycleDB.sqlite \n;Version=3;New=True;Compress=True;");
             raceRepo = new RaceRepo(props);
             participantRepo = new ParticipantRepo(props);
             participantservice = new ParticipantService(participantRepo, raceRepo);
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                // connected = true;
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
                    object response = handleRequest((RequestDto)request);
                    if (response != null)
                    {
                        sendResponse((ResponseDto)response);
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

        private ResponseDto handleRequest(RequestDto request)
        {
            ResponseDto response = null;

            // if (request is RequestDto)
            // {
                Console.WriteLine("SendMessageRequest ...");
                string type = request.RequestType;
                if (type.Equals("addParticipant"))
                {
                    // generate id
                    int lastID = participantservice.GetLasIDParticipant();
                    //add partipant
                    Participant p = request.Dto;
                    int temp;
                    temp = lastID+1;
                    p.Id = temp;
                    Console.WriteLine("P:"+p+" CW99");
                    List<Participant> participants = new List<Participant>();
                    participants.Add(p);
                    participantservice.add(p);
                    Console.WriteLine(p.ToString()+" added CW99");
                     response = new ResponseDto("Participant","Success" , participants);
                    return response;
                }
         // }
            return response;
        }
        
        private void sendResponse(ResponseDto response)
        {
            Console.WriteLine("sending response " + response);
            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }
        }
    }


}