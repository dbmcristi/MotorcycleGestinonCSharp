using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using NetworkingModule.common;
using RepoModule.Repo;
using ServiceModule.Service;

namespace ServerModule.controller
{
    public class SocketController
    {
        private ParticipantRepo participantRepo;
        private ParticipantService participantSerivce;
        private RaceRepo raceRepo;
        private TcpListener server;
        private ClientRpcWorker worker; //toni
        List<TcpClient> clients = new List<TcpClient>();

        public SocketController()
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString",
                "Data Source=D:\\\\FACULTATE_23-24\\\\mpp23-24\\\\gitClones\\\\mpp-proiect-java-dbmcristi\\\\motorcycleDB.sqlite \n;Version=3;New=True;Compress=True;");
            raceRepo = new RaceRepo(props);
            participantRepo = new ParticipantRepo(props);
            participantSerivce = new ParticipantService(participantRepo, raceRepo);
        }

        public void Start()
        {
            IPAddress adr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(adr, 9999);
            server = new TcpListener(ep);
            server.Start();
            while (true)
            {
                Console.WriteLine("Waiting for clients ...");
                // When a new client connects
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Client connected ...");
                processRequest(client);
            }
        }

        public void processRequest(TcpClient client)
        {
            Thread t = createWorker(client);
            t.Start();
        }

        protected Thread createWorker(TcpClient client)
        {
            worker = new ClientRpcWorker(participantSerivce, client);
            return new Thread(worker.run);
          
        }
    }
}