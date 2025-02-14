using Drinks.AuthModule.Models;
using DrinksProject.AuthModule.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DrinksProject.AuthModule.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<UserProfile> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new UserProfile
                {
                    Id = user.Id, // Преобразование в UserProfile (адаптируйте под свои поля)
                    Email = user.Email ?? string.Empty,
                    Username = user.UserName ?? string.Empty
                };
            }

            throw new Exception("User not found");
            
        }
    }
}