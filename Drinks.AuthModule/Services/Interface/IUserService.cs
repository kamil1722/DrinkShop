using Drinks.AuthModule.Models;
using Drinks.AuthModule.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Drinks.AuthModule.Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(RegisterViewModel model);
        Task<UserProfile> GetUserByEmailAsync(string email);
        Task<IdentityUser> FindByEmailAsync(string email);
        string GenerateConfirmationCode();
        Task<bool> StoreConfirmationCodeAsync(string userId, string code);
        Task<(bool Result, string Message)> ConfirmEmailAsync(string userId, string code);
    }
}