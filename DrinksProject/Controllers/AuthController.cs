using DrinksProject.AuthModule.Models;
using DrinksProject.AuthModule.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrinksProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Создайте представление Views/Auth/Login.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = await _authenticationService.LoginAsync(model.Email, model.Password, model.RememberMe);
                if (result)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "User"); // Перенаправление на главную страницу User контроллера
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "User"); // Перенаправьте на главную страницу после выхода
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Создайте представление Views/Auth/Register.cshtml
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
                    // Автоматически войти в систему после регистрации:
                    await _authenticationService.LoginAsync(model.Email, model.Password, false);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}