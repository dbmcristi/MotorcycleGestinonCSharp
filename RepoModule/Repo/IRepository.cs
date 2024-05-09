
using System.Collections.Generic;

namespace RepoModule.Repo
{
    public interface  IRepository<T,ID>
    {
        void Delete(ID id);
          void Add(T e);
          T GetByPos(int pos);
          void Update(T newEntity);
          void DeleteAll();
          List<T> GetAll();
    }
}
