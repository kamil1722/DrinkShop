using Drinks.AuthModule.Models;
using DrinksProject.AuthModule.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DrinksProject.AuthModule.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterViewModel model); // Используем IdentityResult
        Task<UserProfile> GetUserByEmailAsync(string email);
    }
}
