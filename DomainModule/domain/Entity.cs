
using System;
using System.Runtime.Serialization;

namespace DomainModule.domain 
{
    [Serializable()] 
    public class Entity<ID>
    {
        public ID Id { get; set; }

        public Entity(ID id)
        {
            this.Id = id;
        }

        public Entity()
        {
        }

        // public void GetObjectData(SerializationInfo info, StreamingContext context)
        // {
        //     info.AddValue("Id", Id); 
        // }
    }
}