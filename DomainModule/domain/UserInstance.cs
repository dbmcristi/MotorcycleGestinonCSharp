using System;

namespace DomainModule.domain
{
    public class UserInstance
    {
        private static UserInstance Instance;
        private Boolean IsLogged;

        private String User;

        private UserInstance(){}

        public static UserInstance getInstance() {
            if (Instance == null) {
                Instance = new UserInstance();
            }
            return Instance;
        }

        public Boolean isLogged() {
            return IsLogged;
        }

        public void setLogged(Boolean logged) {
            IsLogged = logged;
        }

        public String getUser() {
            return User;
        }

        public void setUser(String user) {
            this.User = user;
        }
    }
}