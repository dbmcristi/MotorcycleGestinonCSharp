
using System;

namespace RepoModule.Repo
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message)
        {
        }

        public RepositoryException(string message, Exception e) : base(message, e)
        {
            Console.WriteLine(e.Message);
        }
    }
}