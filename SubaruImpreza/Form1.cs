using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using DomainModule.domain;
using ServiceModule.Service;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls.WebParts;
using SubaruImpreza;
using NetworkingModule.common;
using Timer = System.Windows.Forms.Timer;

namespace SubaruImpreza
{
    public partial class Form1 : Form, ResponseObserver
    {
        private ParticipantService participantService;
        private UserService userService;
        private RaceService raceService;
        private ClientControllerProtoBuff client;

        private string usrn;

        // Declare a static event
        public static event Action RefreshListView;

        public Form1(ParticipantService participantService, UserService userService,
            RaceService raceService)
        {
            InitializeComponent();
            this.participantService = participantService;
            this.userService = userService;
            this.raceService = raceService;

            listView2.Clear();

            foreach (var VARIABLE in this.participantService.GetAllParticipants())
            {
                listView2.Items.Add(VARIABLE.ToString());
            }

            // Subscribe to the event
            RefreshListView += Form1_RefreshListView;

            //Start a timer to trigger the event every second
            System.Windows.Forms.Timer timer = new Timer();
            timer.Interval = 4000;
            timer.Tick += (s, e) => RefreshListView?.Invoke();
            timer.Start();
        }

        public void setConnectionClient(ClientControllerProtoBuff client)
        {
            Console.WriteLine("setConnectionClient BEGIN");
            this.client = client;
            Console.WriteLine("setConnectionClient END");
        }

        public void setUsrn(string usrn)
        {
            this.usrn = usrn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            foreach (var VARIABLE in userService.getAll())
            {
                listView1.Items.Add(
                    $"User: {{ Id: {VARIABLE.Id}, Name: {VARIABLE.UserName}, Password: {VARIABLE.Password} }}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView2.Clear();
            foreach (var VARIABLE in participantService.GetAllParticipants())
            {
                listView2.Items.Add(VARIABLE.ToString());
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            listView3.Clear();
            foreach (var VARIABLE in raceService.getAll())
            {
                listView3.Items.Add(VARIABLE.ToString());
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }


        //Logout
        private void button5_Click_1(object sender, EventArgs e)
        {
            UserInstance.getInstance().setLogged(false);

            HashSet<string> user = new HashSet<string>();
            user.Add(usrn);
            client.logoutUser(new UserDto(user, "Logout"));
            Dispose();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            //nr of partici-[ants at given race
            int idRace = int.Parse(raceIdSearch.Text);

            int nr = participantService.getNrParticipantsAtRace(idRace);
            nrPartsAtGivenRaceOut.Text = nr.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int idRace = Int32.Parse(raceIdTxt.Text);
            // int id = participantService.GetLasIDParticipant() + 1;
            Participant participant = new Participant(0, nameTxt.Text, teamTxt.Text, idRace);
            // participant.Id = id;
            client.addParticipant(participant);

            Console.WriteLine(participant + " was sended");
            // Connect the socket to the remote endpoint. Catch any errors.
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listView2.Clear();
            foreach (var VARIABLE in participantService.getAllByTeamName(searchTxt.Text))
            {
                listView2.Items.Add(VARIABLE.ToString());
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String input = comboBox1.Text;
            Console.WriteLine(input);
            listView3.Clear();
            foreach (var VARIABLE in raceService.getAllByEngCap(input))
            {
                listView3.Items.Add(VARIABLE.ToString());
            }
        }

//        public void handleRespone(IResponse resp)
        public void handleResponeParticipant(IResponse resp)
        {
            Console.WriteLine("handleing Participant response: " + resp);
            ResponseDto response = (ResponseDto)resp;
            string type = response.ResponseType;
            string message = response.Message;
            if (type.Equals("Participant"))
            {
                Participant result = response.Participants[0];
                if (result != null)
                {
                    Form1_RefreshListView();
                    //listView2.Items.Add(result.ToString());
                    // RefreshListView.Invoke();
                    Console.WriteLine("participant inscris :) " + response);
                }
                else
                {
                    MessageBox.Show("Err", "Error", MessageBoxButtons.OK);
                }
            }
        }

        public void handleResponeUser(IResponse response)
        {
            throw new NotImplementedException();
        }

        public void Form1_RefreshListView()
        {
            // Refresh the ListView
            // listView2.Refresh();
            listView2.Clear();
            foreach (var VARIABLE in participantService.GetAllParticipants())
            {
                listView2.Items.Add(VARIABLE.ToString());
            }
        }
    }
}