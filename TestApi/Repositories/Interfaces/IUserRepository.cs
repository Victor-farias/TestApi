using TestApi.Models;

namespace TestApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string username); 
    }
}
