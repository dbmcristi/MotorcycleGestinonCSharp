using System;
using System.Runtime.Serialization;

namespace DomainModule.domain
{
    
    public class Race : Entity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EngineCapacity EngineCap { get; set; }

        public Race(int id, string name, EngineCapacity engineCap) : base(id)
        {
            Id = id;
            Name = name;
            EngineCap = engineCap;
        }

        public int getId()
        {
            throw new NotImplementedException();
        }
        protected Race(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            EngineCap = (EngineCapacity)info.GetValue("EngineCap", typeof(EngineCapacity));
        }
        public override string ToString()
        {
            return $"Race: {{ Id: {Id}, Name: {Name}, EngineCap: {EngineCap} }}";
        }

    }
}