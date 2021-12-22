
using System.Linq;
using TestApi.Enums;
using TestApi.Repositories.Interfaces;
using TestApi.Services.Interfaces;

namespace TestApi.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository userRepository;

        public AuthorizationService (IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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
