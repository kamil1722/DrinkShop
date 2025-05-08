using System.ComponentModel.DataAnnotations;

namespace WebStore.AuthModule.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; } = false;
    }
}