using System;
using System.Collections.Generic;
using NetworkingModule.common;

namespace SubaruImpreza
{
    public class ParticipantSubject : Subject
    {
        private List<ResponseObserver> list = new List<ResponseObserver>();
        private ResponseDto dto;

        public ParticipantSubject(ResponseDto dto)
        {
            this.dto = dto;
        }

        public void addObserver(ResponseObserver observer)
        {
          list.Add(observer);
        }

        public void notifyObserver()
        {   Console.WriteLine("Notifying Participant Observer...");
            foreach (var VARIABLE in list)
            {
                
                VARIABLE.handleResponeParticipant(dto);
                //VARIABLE.handleRespone(dto);
            }
        }
    }
}