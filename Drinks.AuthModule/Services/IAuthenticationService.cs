using Drinks.AuthModule.Models;
using System.Threading.Tasks;

namespace DrinksProject.AuthModule.Services
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<UserProfile> GetCurrentUserAsync();
        bool IsAuthenticated();
    }
}