using Drinks.AuthModule.Models;
using Drinks.AuthModule.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Drinks.AuthModule.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;   
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager; // ASP.NET Identity

        public AuthenticationService(IUserService userService, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }
        public async Task<bool> LoginAsync(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            //  Используем SignInManager для входа в систему с ASP.NET Identity:
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user.Username, password, rememberMe, lockoutOnFailure: false);

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