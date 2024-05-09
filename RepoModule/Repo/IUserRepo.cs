using System;
using DomainModule.domain;

namespace RepoModule.Repo
{
    public interface IUserRepo : IRepository<User,Int32>
    {
        
    }
}