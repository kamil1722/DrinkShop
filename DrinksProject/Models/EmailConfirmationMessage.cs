// DrinksProject/Models/EmailConfirmationMessage.cs
namespace DrinksProject.Models
{
    public class EmailConfirmationMessage
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Code { get; set; }
    }
}