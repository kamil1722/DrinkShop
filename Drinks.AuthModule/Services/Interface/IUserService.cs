using Drinks.AuthModule.Models;
using Drinks.AuthModule.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Drinks.AuthModule.Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterViewModel model); // Используем IdentityResult
        Task<UserProfile> GetUserByEmailAsync(string email);
    }
}
