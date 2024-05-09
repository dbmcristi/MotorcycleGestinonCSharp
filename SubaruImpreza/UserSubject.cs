using System;
using System.Collections.Generic;
using NetworkingModule.common;

namespace SubaruImpreza
{
    public class UserSubject : Subject
    {
        private List<ResponseObserver> list = new List<ResponseObserver>();
        private UserDto dto;

        public UserSubject(UserDto dto)
        {
            this.dto = dto;
        }

        public void addObserver(ResponseObserver observer)
        {
            list.Add(observer);
        }

        public void notifyObserver()
        {
            Console.WriteLine("Notyfing User Observer...");
            foreach (var VARIABLE in list)
            {
                //
                VARIABLE.handleResponeUser(dto);

               // VARIABLE.handleRespone(dto);
            }
        }
    }
}