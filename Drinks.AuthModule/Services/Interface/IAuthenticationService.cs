using Drinks.AuthModule.Models;

namespace Drinks.AuthModule.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<UserProfile> GetCurrentUserAsync();
        bool IsAuthenticated();
    }
}