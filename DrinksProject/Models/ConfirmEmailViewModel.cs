// DrinksProject/Models/ConfirmEmailViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace DrinksProject.Models
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}