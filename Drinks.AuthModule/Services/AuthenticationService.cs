using Drinks.AuthModule.Models;
using DrinksProject.AuthModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Drinks.AuthModule.Services
{
    public class AuthenticationService(IUserService userService, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager) : IAuthenticationService
    {
        private readonly IUserService _userService = userService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager; // ASP.NET Identity

        public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            //  Используем SignInManager для входа в систему с ASP.NET Identity:
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UserProfile> GetCurrentUserAsync()
        {
            var user = await _signInManager.UserManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user != null)
            {
                return await _userService.GetUserByEmailAsync(user.Email ?? string.Empty); // Конвертируем в UserProfile
            }

             throw new Exception("User not found");
        }

        public bool IsAuthenticated()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity;
            return identity != null && identity.IsAuthenticated;
        }
    }
}