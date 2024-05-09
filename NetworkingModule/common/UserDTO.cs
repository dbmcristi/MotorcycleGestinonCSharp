using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DomainModule.domain;

namespace NetworkingModule.common
{
    [Serializable()]
    public class UserDto : IRequest, IResponse, ISerializable

    {
        public HashSet<string> Username { get; set; }
        public string Message { get; set; }

        public UserDto(HashSet<string> username, string message)
        {
            Username = username;
            Message = message;
        }

        public UserDto()
        {
        }

// The special constructor is used to deserialize values.
        public UserDto(SerializationInfo info, StreamingContext context)
        {
            // Get the set values from SerializationInfo and assign them to the appropriate properties.
            Username = (HashSet<string>)info.GetValue("Username", typeof(HashSet<string>));
            Message = info.GetString("Message");
        }

        public override string ToString()
        {
            string text = "";
            foreach (var VARIABLE in Username)
            {
                text = text + " " + VARIABLE;
            }

            return $"UserDto{{username={text}}}";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Message", Message);
        }
    }
}