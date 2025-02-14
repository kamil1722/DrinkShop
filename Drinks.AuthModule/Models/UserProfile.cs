namespace Drinks.AuthModule.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty; // Тип должен соответствовать типу идентификатора пользователя в вашей БД
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        // Другие свойства профиля
    }
}