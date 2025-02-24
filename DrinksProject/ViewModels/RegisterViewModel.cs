namespace DrinksProject.ViewModels
{
    public class RegisterViewModel : Drinks.AuthModule.ViewModels
        .RegisterViewModel
    {
        //[Display(Name = "Я согласен с условиями использования")]
        //[Required(ErrorMessage = "Необходимо согласиться с условиями использования")]
        //[Range(typeof(bool), "true", "true", ErrorMessage = "Необходимо согласиться с условиями использования")] // Для валидации на стороне сервера
        //public bool TermsAccepted { get; set; }
    }
}