using System;
using System.Collections.Generic;
using System.Data.SQLite;
using DomainModule.domain;
using JDBCUtilsModule;


// using log4net;
// using log4net.Config;
// using SubaruImpreza.domain;

namespace RepoModule.Repo
{

    public class RaceRepo : IRaceRepo
    {


        // private static readonly ILog log = LogManager.GetLogger("RaceRepo");


        IDictionary<String, string> props;
        private SQLiteConnection con;

        public RaceRepo(IDictionary<String, string> props)
        {
            // log.Info("Creating SortingTaskDbRepository ");
            this.con = DBUtils.getConnection(props);
            this.props = props;
            // BasicConfigurator.Configure();

        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(Race entity)
        {
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into race  values (@id, @name, @engineCapacity)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                var paramElems = comm.CreateParameter();
                paramElems.ParameterName = "@name";
                paramElems.Value = entity.Name;
                comm.Parameters.Add(paramElems);

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@engineCapacity";
                paramDesc.Value = entity.EngineCap.ToString();
                comm.Parameters.Add(paramDesc);


                var result = comm.ExecuteNonQuery();
                // log.InfoFormat("Exiting {0} with succes", "addRace");

                if (result == 0)
                    throw new RepositoryException("No Race added !");

            }
        }

        public Race GetByPos(int pos)
        {
            throw new NotImplementedException();
        }

        public void Update(Race newEntity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public int GetLastID()
        {
            int id = -1;
            List<Race> tasksR = new List<Race>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select MAX(id) from Race ";

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        id = dataR.GetInt32(0);
                    }

                }

                // log.InfoFormat("Exiting getLastID Race with succes ");

                return id;
            }
        }

        public List<Race> GetAll()
        {
            List<Race> tasksR = new List<Race>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id,name,engineCapacity from race ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String name = dataR.GetString(1);
                        String engCap = dataR.GetString(2);

                        EngineCapacity engineCapacity = (EngineCapacity)Enum.Parse(typeof(EngineCapacity), engCap);
//                    Console.WriteLine("weekday1:"+engineCapacity);
                        Race user = new Race(id, name, engineCapacity);
                        tasksR.Add(user);
                    }
                }
            }

            // log.InfoFormat("Exiting getAll Race with succes ");

            return tasksR;
        }
    }
}