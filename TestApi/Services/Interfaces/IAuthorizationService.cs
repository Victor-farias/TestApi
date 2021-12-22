using System;

namespace TestApi.Services.Interfaces
{
    public interface IAuthorizationService : IDisposable
    {
        bool HasAdminRole(string userName);
    }
}
