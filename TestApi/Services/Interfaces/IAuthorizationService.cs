namespace TestApi.Services.Interfaces
{
    public interface IAuthorizationService
    {
        bool HasAdminRole(string userName);
    }
}
