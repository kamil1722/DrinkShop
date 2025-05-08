using System.ComponentModel.DataAnnotations;

namespace WebStore.AuthModule.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязателен для заполнения")]
        [Display(Name = "Имя пользователя")]
        [DataType(DataType.Text)] // Consider adding DataType for better user experience on some UI frameworks
        public string UserName { get; set; } = string.Empty; // Initialize to empty string

        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)] // Consider adding DataType for better user experience on some UI frameworks
        public string Email { get; set; } = string.Empty; // Initialize to empty string

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty; // Initialize to empty string

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение не совпадают.")]
        [Required(ErrorMessage = "Подтверждение пароля обязательно для заполнения")]
        public string ConfirmPassword { get; set; } = string.Empty; // Initialize to empty string
    }
}