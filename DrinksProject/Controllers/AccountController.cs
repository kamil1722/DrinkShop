using Drinks.AuthModule.Services.Interface;
using DrinksProject.Models;
using DrinksProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

                if (result.Succeeded)
                {
                    // После успешной регистрации, выполняем вход пользователя через AuthenticationService
                    if (await _authenticationService.LoginAsync(model.Email, model.Password, false))
                    {

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

        [HttpPost]
        public async Task<IActionResult> SendCodeToEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { success = false, message = "Неверный формат email.  Email не может быть пустым." });
                }

                if (!new EmailAddressAttribute().IsValid(email))
                {
                    return Json(new { success = false, message = "Неверный формат email." });
                }

                var user = await _userService.FindByEmailAsync(email);

                string confirmationCode = _userService.GenerateConfirmationCode();

                var messageJson = _rabbitMQService.GetEmailMessageJson(confirmationCode, email);

                await _userService.StoreConfirmationCodeAsync(user.Id, confirmationCode);

                _rabbitMQService.SendMessage(_configuration["RabbitMQ:QueueName"], messageJson);

                return Json(new { success = true, message = "Код подтверждения успешно отправлен." });
            }
            catch
            {
                return Json(new { success = false, message = "Произошла ошибка на сервере." });
            }
        }

        [HttpGet]
        [Authorize] // Требуется аутентификация
        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _authenticationService.GetCurrentUserAsync();

                if (currentUser == null)
                {
                    return RedirectToAction("Logout", "Account"); 
                }

                var codeValid = await _userService.ConfirmEmailAsync(currentUser.Id, model.Code);

                if (codeValid.Result)
                {
                    ViewBag.ConfirmationSuccess = true;
                    return View(); 
                }
                else
                {
                    ViewBag.ConfirmationSuccess = false;
                    ModelState.AddModelError(string.Empty, codeValid.Message);
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
            var currentUser = _authenticationService.GetCurrentUserAsync().Result;

            var model = new UserViewModel
            {
                Username = currentUser.Username,
                Email = currentUser.Email,
                EmailConfirmed = currentUser.EmailConfirmed
            };

            return View(model);
        }
    }
}