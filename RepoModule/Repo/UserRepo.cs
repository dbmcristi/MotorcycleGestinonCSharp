using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModule.domain;
using JDBCUtilsModule;


namespace RepoModule.Repo
{
    public class UserRepo : IUserRepo
    {
        private IDictionary<String, string> props;
        private SQLiteConnection con;

        public UserRepo(IDictionary<String, string> props)
        {
            //log.Info("Creating SortingTaskDbRepository ");
            this.con = DBUtils.getConnection(props);
            this.props = props;
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        
        public bool isLogged(String usrn, String pwsd) {
            int id = -1;
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select count(id) from user where username=@usrn and password=@pwsd ";

                var paramUsrn = comm.CreateParameter();
                paramUsrn.ParameterName = "@usrn";
                paramUsrn.Value =usrn;
                comm.Parameters.Add(paramUsrn);
                var paramPwd = comm.CreateParameter();
                paramPwd.ParameterName = "@pwsd";
                paramPwd.Value = pwsd;
                comm.Parameters.Add(paramPwd);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        id = dataR.GetInt32(0);
                    }
                }

                if (id == 0)
                {
                    return false;
                }

                return true;
            }
        }
        public void Add(User entity)
        {
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into user  values (@id, @username, @password)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                var paramElems = comm.CreateParameter();
                paramElems.ParameterName = "@username";
                paramElems.Value = entity.UserName;
                comm.Parameters.Add(paramElems);

                var paramDesc = comm.CreateParameter();
                paramDesc.ParameterName = "@password";
                paramDesc.Value = entity.Password;
                comm.Parameters.Add(paramDesc);


                var result = comm.ExecuteNonQuery();
                if (result == 0)
                    throw new RepositoryException("No task added !");
            }
        }

        public User GetByPos(int pos)
        {
            throw new NotImplementedException();
        }

        public void Update(User newEntity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            List<User> tasksR = new List<User>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select id,username,password from user ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        String username = dataR.GetString(1);
                        String password = dataR.GetString(2);
                        User user = new User(id, username, password);
                        tasksR.Add(user);
                    }
                }
            }

            return tasksR;
        }

        public int GetLastID()
        {
            int id = -1;
            List<User> tasksR = new List<User>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select MAX(id) from user ";
                
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
    }
}