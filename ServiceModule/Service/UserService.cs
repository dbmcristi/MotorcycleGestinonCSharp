using System;
using System.Collections.Generic;
using DomainModule.domain;
using RepoModule.Repo;

namespace ServiceModule.Service
{
    public class UserService
    {
        private UserRepo userRepo;

        public UserService(UserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public bool ValidateUser(User user)
        {
            return false;
        }

        public bool isLogged(String usrn, String pwsd) {
            try {
                return userRepo.isLogged(usrn,pwsd);
            } catch (RepositoryException e) {
                throw new Exception(e.ToString());
            }
        }

        public void add(User user)
        {
            userRepo.Add(user);
        }

        public List<User> getAll()
        {
            return userRepo.GetAll();
        }

        public int getLastId()
        {
            return userRepo.GetLastID();
        } 
        public void Logout()
        {
        }
    }
}