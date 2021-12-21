using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestApi.Data;
using TestApi.Models;
using TestApi.Repositories.Interfaces;

namespace TestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext database;

        public UserRepository(ApplicationDbContext database)
        {
            this.database = database;
        }

        public User GetUser(string userName)
        {
            var user = database.Users.Include(u => u.UserRoles).
                Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}
