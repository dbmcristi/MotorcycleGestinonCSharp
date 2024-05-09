using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DomainModule.domain;

using NetworkingModule.common;
using SubaruImpreza;

namespace SubaruImpreza
{
    public class ClientController
    {
        private Form form;
        private Form formLogin;
        private volatile bool finished;
        private TcpClient connection;
        private NetworkStream stream;
        private IFormatter formatter;

        public ClientController(Form form1, Form formLogin)
        {
            this.form = form1;
            this.formLogin = formLogin;
            initializeConnection();             
        }
        
        private void initializeConnection()
        { 
            try
            {
                connection = new TcpClient("127.0.0.1", 9999);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                // _waitHandle = new AutoResetEvent(false);
                startReader();
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
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (response is ResponseDto)
                    {
                        Console.WriteLine("Raspuns primit din Server, facem handleResponse");
                        handleResponse((ResponseDto)response);
                    } else if (response is UserDto)
                    {
                        Console.WriteLine("Raspuns primit lista user din Server, facem handleResponse");
                        handleResponse((UserDto)response);
                    }
                    else
                    {
                        // lock (responses)
                        // {
                        //     responses.Enqueue((Response)response);
                        // }
                        //
                        // _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }

        private void sendRequest(IRequest request)
        {     Console.WriteLine("sendRequest BEGIN");
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object " + e);
            }
            Console.WriteLine("sendRequest END");
        }

        private void handleResponse( ResponseDto response)
        {
            string type = response.ResponseType;
            string message = response.Message;
            if (message.Equals("Success") )
            {
                ParticipantSubject participantSubject = new ParticipantSubject(response);
                //add ui controller
                participantSubject.addObserver((ResponseObserver)form);
                participantSubject.notifyObserver();
            }
        }
        
        private void handleResponse( UserDto response)
        { Console.WriteLine("handleResponse UserDto begin");
            string message = response.Message;
            if (message.Equals("Success") )
            {
                UserSubject userSubject = new UserSubject(response);
                //add ui controller
                userSubject.addObserver((ResponseObserver)formLogin);
                userSubject.notifyObserver();
            }
            Console.WriteLine("handleResponse UserDto END");
        }
        
        public virtual void addParticipant(Participant participant)
        {
            RequestDto request = new RequestDto("addParticipant", participant);
            sendRequest(request);
        }
        
        public virtual void addUser(UserDto user)
        {   Console.WriteLine("addUser BEGIN");
            sendRequest(user); 
            Console.WriteLine("addUser END");
        }
        
        public virtual void logoutUser(UserDto user)
        {   Console.WriteLine("logoutUser BEGIN");
            sendRequest(user); 
            Console.WriteLine("logoutUser END");
        }
    }
}