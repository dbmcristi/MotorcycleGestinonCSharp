using System;
using System.Collections.Generic;
using System.Data.SQLite;
using DomainModule.domain;
using JDBCUtilsModule;

namespace RepoModule.Repo
{
    public class ParticipantRepo : IParticipantRepo
    {
        private IDictionary<String, string> props;
        private SQLiteConnection con;

        public ParticipantRepo(IDictionary<String, string> props)
        {
            this.con = DBUtils.getConnection(props);
            this.props = props;
        }

        public void Delete(int id)
        {
            // Implementation for deleting a participant
        }

        public void Add(Participant entity)
        {
            // Implementation for adding a participant
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into participant  values (@id, @name, @teamName, @idRace)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                var paramElems = comm.CreateParameter();
                paramElems.ParameterName = "@name";
                paramElems.Value = entity.Name;
                comm.Parameters.Add(paramElems);

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@teamName";
                paramDesc.Value = entity.TeamName;
                comm.Parameters.Add(paramDesc);

                var paramDesc2 = comm.CreateParameter();
                paramDesc2.ParameterName = "@idRace";
                paramDesc2.Value = entity.IdRace;
                comm.Parameters.Add(paramDesc2);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
        }

        public Participant GetByPos(int pos)
        {
            // Implementation for retrieving a participant by position
            return null; // Replace with actual logic
        }

        public void Update(Participant newEntity)
        {
            // Implementation for updating a participant
        }

        public void DeleteAll()
        {
            // Implementation for deleting all participants
        }

        public int GetLastID()
        {
            int id = -1;
            List<User> tasksR = new List<User>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select MAX(id) from participant ";
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        id = dataR.GetInt32(0);
                    }
                }
                return id;
            }
        }

        public List<Participant> GetAll()
        {
            // Implementation for retrieving all participants
            List<Participant> tasksR = new List<Participant>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id,name,teamName,idRace from participant";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String name = dataR.GetString(1);
                        String teamName = dataR.GetString(2);
                        int idRace = dataR.GetInt32(3);

                        Participant participant = new Participant(id, name, teamName, idRace);
                        tasksR.Add(participant);
                    }
                }
            }

            return tasksR;
        }

        public void registerParticipant(int idParticipant, string name, string teamName, int idRace)
        {
            string query =
                "INSERT INTO Participant (id, name, teamName, idRace) VALUES (@id, @name, @teamName, @idRace)";

            try
            {
                // using (var connection = new SQLiteConnection(ConfigurationManager.ConnectionStrings["SQLiteDBConnectionString"].ConnectionString))
                // using (var connection = new SQLiteConnection("Data Source=C:/Users/tonyc/Downloads/Anul2/Semestrul_2/MPP/databases/CarsDB-JavaHomework"))
                // {
                // connection.Open();
                Console.WriteLine(con.ConnectionString);
                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@id", idParticipant);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@teamName", teamName);
                    command.Parameters.AddWithValue("@idRace", idRace);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Am adaugat cu succes un nou participant!");
                }
                // }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("erroare la adaugare participant in DB" + e.ToString());
            }
        }
    }
}