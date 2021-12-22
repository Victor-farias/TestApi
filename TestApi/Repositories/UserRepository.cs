using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using TestApi.Data;
using TestApi.Models;
using TestApi.Repositories.Interfaces;

namespace TestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private SafeHandle handle;
        private bool disposed = false;
        private readonly ApplicationDbContext database;

        public UserRepository(ApplicationDbContext database)
        {
            this.database = database;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    if (handle != null)
                    {
                        handle.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public User GetUser(string userName)
        {
            var user = database.Users.Include(u => u.UserRoles).
                Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}
