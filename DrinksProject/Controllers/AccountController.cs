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

                await _userService.StoreConfirmationCodeAsync(user.Id, confirmationCode);

                var messageJson = _rabbitMQService.GetEmailMessageJson(confirmationCode, email);

                _rabbitMQService.SendMessage(_configuration["RabbitMQ:QueueName"], messageJson);

                return Json(new { success = true, message = "Код подтверждения успешно отправлен." });
            }
            catch
            {
                return Json(new { success = false, message = "Произошла ошибка на сервере." });
            }
        }

        [HttpPost]
        public async Task<bool> ConfirmEmail(ConfirmEmailViewModel model)
        {

            ///ConfirmEmail
            bool isCodeValid = await _userService.ConfirmEmailAsync(model.UserId, model.Code);

            if (isCodeValid)
            {
                ViewBag.ConfirmationSuccess = true;
            }
            else
            {
                ViewBag.ConfirmationSuccess = false;
                ModelState.AddModelError(string.Empty, "Неверный код подтверждения.");
            }

            return ViewBag.ConfirmationSuccess;
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
                Email = currentUser.Email
            };

            return View(model);
        }
    }
}