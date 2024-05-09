using System;

namespace DomainModule.domain
{
    public class User : Entity<Int32>
    {
        public int Id { get; set; }
        public String Password { get; set; }
        public String UserName { get; set; }

        public User(int id, string password, String userName) : base(id)
        {
            Id = id;
            Password = password;
            UserName = userName;
        }

        public override string ToString()
        {
            return $"User: {{ Id: {Id}, Name: {UserName}, Password: {Password} }}\n";
        }

    }
}