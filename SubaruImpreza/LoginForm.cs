using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DomainModule.domain;
using SubaruImpreza;
using NetworkingModule.common;
using ServiceModule.Service;


namespace SubaruImpreza
{
    public partial class LoginForm : Form, ResponseObserver
    {
        private UserService userService;

        private List<String> userList;
        private readonly ParticipantService participantService;
        private readonly RaceService raceService;

        public LoginForm(ParticipantService participantService, UserService userService, RaceService raceService)
        {
            InitializeComponent();
            this.userService = userService;
            this.participantService = participantService;
            this.raceService = raceService;
        }
//login
        private void button1_Click(object sender, EventArgs e)
        {
            String usrn = textBox1.Text;
            String pwsd = textBox2.Text;
            bool IsLogged = userService.isLogged(usrn, pwsd);

            UserInstance.getInstance().setLogged(IsLogged);

            Form mainStage = new Form1(participantService, userService, raceService);
            if (IsLogged)
            {
                HashSet<string> user = new HashSet<string>();
                user.Add("Login" + usrn);

                ClientControllerProtoBuff client = new ClientControllerProtoBuff(mainStage, this);
                ((Form1)mainStage).setConnectionClient(client);
                UserInstance.getInstance().setUser(usrn);
                ((Form1)mainStage).Text = "Welcome "+ usrn;
                ((Form1)mainStage).setUsrn(usrn);
                mainStage.Show();
                client.addUser(new UserDto(user, "addUser"));
            }
            else
            {
                String message = "Invalid user or password";
                MessageBox.Show(message, "Error", MessageBoxButtons.OK);
                mainStage.Dispose();
            }

        }
        public void handleResponeParticipant(IResponse response)
        {
            throw new NotImplementedException();
        }
        
        //public void handleRespone(IResponse resp)
        public void handleResponeUser(IResponse resp)
        {
            Console.WriteLine("Handleing User Response: " + resp);
            UserDto response = (UserDto)resp;
            string message = response.Message;
            if (message.Equals("Success"))
            {
                HashSet<string> result = response.Username;
                Console.WriteLine(result + " --- LoginForm");
                if (result != null)
                {
                    string text = "";
                    foreach (var VARIABLE in result)
                    {
                        text = text + " " + VARIABLE;
                    }

                    Console.WriteLine("Useri activi :) " + text);
                    MessageBox.Show(text, "Information", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Err", "Error", MessageBoxButtons.OK);
                }
            }
        }

       
    }
}