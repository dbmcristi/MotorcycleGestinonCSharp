using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DomainModule.domain;

namespace NetworkingModule.common
{
    [Serializable()]
    public class ResponseDto : IResponse, ISerializable
    {
        public string ResponseType { get; set; }
        public string Message { get; set; }
        public List<Participant> Participants { get; set; }

        public ResponseDto(string responseType, string message, List<Participant> participants)
        {
            ResponseType = responseType;
            Message = message;
            Participants = participants;
        }

        public override string ToString()
        {
            return $"ResponseDto{{responseType='{ResponseType}', message='{Message}', participants='{Participants}'}}";
        }

        public ResponseDto()
        {
        }

        // The special constructor is used to deserialize values.
        public ResponseDto(SerializationInfo info, StreamingContext context)
        {
            // Get the set values from SerializationInfo and assign them to the appropriate properties.
            ResponseType = info.GetString("ResponseType");
            Message = info.GetString("Message");
            Participants = (List<Participant>)info.GetValue("Participants", typeof(List<Participant>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ResponseType", ResponseType);
            info.AddValue("Message", Message);
            info.AddValue("Participants", Participants);
        }
    }
}