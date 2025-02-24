using Drinks.AuthModule.Models;
using Drinks.AuthModule.Services.Interface;
using Drinks.AuthModule.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Drinks.AuthModule.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _memoryCache; // Inject IMemoryCache
        private const string ConfirmationCodeCacheKey = "ConfirmationCode_";

        public UserService(UserManager<IdentityUser> userManager, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;

        }

        public async Task<IdentityResult> CreateUserAsync(RegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
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

            return null;
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }


        public string GenerateConfirmationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> StoreConfirmationCodeAsync(string userId, string code)
        {
            try
            {
                var expirationTime = DateTimeOffset.UtcNow.AddSeconds(60);

                await Task.Run(() => _memoryCache.Set(ConfirmationCodeCacheKey + userId, code, expirationTime));

                return true;
            }
            catch (Exception)
            {
                return false; // Indicate failure to store
            }
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string code)
        {
            try
            {
                string cachedCode = await Task.Run(() => _memoryCache.Get<string>(ConfirmationCodeCacheKey + userId) ?? string.Empty);

                if (!string.IsNullOrEmpty(cachedCode) && cachedCode == code)
                {
                    // Удалить код из кэша, чтобы предотвратить повторное использование
                    await Task.Run(() => _memoryCache.Remove(ConfirmationCodeCacheKey + userId));

                    // Здесь вы обычно обновляете свойство EmailConfirmed пользователя
                    // Например, если у вас есть пользовательский класс User, вы можете сделать это:
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        user.EmailConfirmed = true; 
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return false; 
                        }
                    }

                    return true; 
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}