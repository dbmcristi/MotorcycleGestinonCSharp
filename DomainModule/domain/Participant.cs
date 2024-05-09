using System;
using System.Runtime.Serialization;

namespace DomainModule.domain
{
    [Serializable()]
    public class Participant:Entity<int>,ISerializable
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string Name { get; set; }
        public int IdRace { get; set; }

        // public Participant(int id) : base(id)
        // {
        // }


        public Participant(int id, string teamName, string name, int idRace):base(id)
        {
            Id = id;
            TeamName = teamName;
            Name = name;
            IdRace = idRace;
        }
        protected Participant(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Id");
            TeamName = info.GetString("TeamName");
            Name = info.GetString("Name");
            IdRace = info.GetInt32("IdRace");
        }

        public int getId()

        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"Participant: {{ Id: {Id}, TeamName: {TeamName}, Name: {Name}, IdRace: {IdRace} }}";
        }  

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("TeamName", TeamName);
            info.AddValue("Name", Name);
            info.AddValue("IdRace", IdRace);
        }
    }
}