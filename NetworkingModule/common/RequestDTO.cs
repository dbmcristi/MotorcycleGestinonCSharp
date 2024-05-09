using System;
using System.Runtime.Serialization;
using DomainModule.domain;

namespace NetworkingModule.common
{
    [Serializable()]
    public class RequestDto:IRequest, ISerializable
    {
        public RequestDto()
        {
        }

        public string RequestType { get; }
        public Participant Dto { get; }

        public RequestDto(string requestType, Participant dto)
        {
            RequestType = requestType;
            Dto = dto;
        }
        protected RequestDto(SerializationInfo info, StreamingContext context)
        {
            RequestType = info.GetString("RequestType");
            Dto = (Participant)info.GetValue("Dto", typeof(Participant));
        }
        public override string ToString()
        {
            return $"RequestDto{{requestType='{RequestType}', dto={Dto}}}";
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("RequestType", RequestType);
            info.AddValue("Dto", Dto);        }
    }
}