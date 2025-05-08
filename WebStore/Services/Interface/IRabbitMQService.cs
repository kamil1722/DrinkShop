public interface IRabbitMQService
{
    void SendMessage(string queueName, string message);
    string GetEmailMessageJson(string token, string email);
}