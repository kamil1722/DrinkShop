using Drinks.AuthModule.Services.Interface;
using DrinksProject.Models;
using DrinksProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DrinksProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IAuthenticationService authenticationService,
            IRabbitMQService rabbitMQService, IConfiguration configuration)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _rabbitMQService = rabbitMQService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);
                var user = await _userService.FindByEmailAsync(model.Email);

                if (result.Succeeded)
                {
                    var confirmationToken = await _userService.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = confirmationToken }, Request.Scheme);

                    var messageJson = _rabbitMQService.GetEmailMessageJson(confirmationLink, model.Email);

                    // После успешной регистрации, выполняем вход пользователя через AuthenticationService
                    if (await _authenticationService.LoginAsync(model.Email, model.Password, false))
                    {
                        // Send the message to RabbitMQ
                        _rabbitMQService.SendMessage(_configuration["RabbitMQ:QueueName"], messageJson);

                        return RedirectToAction("Index", "User"); // Перенаправление на главную страницу
                    }
                    else
                    {
                        ModelState.AddModelError("", "Не удалось выполнить вход после регистрации.");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (await _authenticationService.LoginAsync(model.Email, model.Password, model.RememberMe))
                {
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError("", "Неправильный логин или пароль.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        [Authorize] // Требуется аутентификация
        public IActionResult Manage()
        {
            // Здесь можно добавить логику для отображения страницы управления пользователем
            return View();
        }
    }
}