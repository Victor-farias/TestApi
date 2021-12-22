using System;
using TestApi.Models;

namespace TestApi.Repositories.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        User GetUser(string username); 
    }
}
