// DrinksProject/Models/ConfirmEmailViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace DrinksProject.Models.EmailModels
{
    public class ConfirmEmailViewModel
    {
        [Required(ErrorMessage = "Код подтверждения обязателен.")]
        [Display(Name = "Код подтверждения")]
        public string Code { get; set; }
    }
}