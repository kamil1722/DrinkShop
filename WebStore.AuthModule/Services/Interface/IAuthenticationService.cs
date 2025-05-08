using WebStore.AuthModule.Models;

namespace WebStore.AuthModule.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<UserProfile> GetCurrentUserAsync();
        bool IsAuthenticated();
    }
}