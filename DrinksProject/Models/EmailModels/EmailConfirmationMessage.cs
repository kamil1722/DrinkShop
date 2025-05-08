// DrinksProject/Models/EmailConfirmationMessage.cs
namespace DrinksProject.Models.EmailModels
{
    public class EmailConfirmationMessage
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Code { get; set; }
    }
}