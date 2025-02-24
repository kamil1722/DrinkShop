using Drinks.AuthModule.Models;

namespace DrinksProject.ViewModels
{
    public class UserViewModel : UserProfile
    {
        public bool IsEmailConfirmed { get; set; }
    }
}
