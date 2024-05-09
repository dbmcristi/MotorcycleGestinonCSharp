using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepoModule.Repo;
using ServiceModule.Service;

namespace SubaruImpreza
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString","Data Source=D:\\\\FACULTATE_23-24\\\\mpp23-24\\\\gitClones\\\\mpp-proiect-java-dbmcristi\\\\motorcycleDB.sqlite \n;Version=3;New=True;Compress=True;");

            UserRepo userRepo = new UserRepo(props);
            UserService userService = new UserService(userRepo);

            RaceRepo raceRepo = new RaceRepo(props);
            RaceService raceService = new RaceService(raceRepo);

            ParticipantRepo participantRepo = new ParticipantRepo(props);
            ParticipantService participantService = new ParticipantService(participantRepo, raceRepo);
            
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1(participantService,userService,raceService));
            
            Application.Run(new LoginForm(participantService,userService,raceService));
        }
    }
}