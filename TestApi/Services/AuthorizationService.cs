
using System;
using System.Linq;
using System.Runtime.InteropServices;
using TestApi.Enums;
using TestApi.Repositories.Interfaces;
using TestApi.Services.Interfaces;

namespace TestApi.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private SafeHandle handle;
        private bool disposed = false;
        private readonly IUserRepository userRepository;

        public AuthorizationService (IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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

        public bool HasAdminRole(string userName)
        {
            var user = userRepository.GetUser(userName);

            if (user != null)
            {
               return user.UserRoles.Any(u => u.Id == (int)RolesEnum.ADMIN);
            }

            return false;
        }
    }
}
