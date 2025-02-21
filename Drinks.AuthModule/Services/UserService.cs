﻿using Drinks.AuthModule.Models;
using Drinks.AuthModule.Services.Interface;
using Drinks.AuthModule.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Drinks.AuthModule.Services
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

        public async Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
    }
}