// DrinksProject/Services/RabbitMQService.cs
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using DrinksProject.Models;
using DrinksProject.Models.EmailModels;

public class RabbitMQService : IRabbitMQService
{
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly string _username;
    private readonly string _password;

    public RabbitMQService(IConfiguration configuration)
    {
        _hostname = configuration["RabbitMQ:Hostname"];
        _queueName = configuration["RabbitMQ:QueueName"];
        _username = configuration["RabbitMQ:Username"];
        _password = configuration["RabbitMQ:Password"];
    }

    public void SendMessage(string queueName, string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    public string GetEmailMessageJson(string confirmationCode, string email)
    {
        var emailBody = $"Ваш код подтверждения: {confirmationCode}";

        var message = new EmailConfirmationMessage
        {
            ToEmail = email,
            Subject = "Подтвердите свой email",
            Body = emailBody,
            Code = confirmationCode // Include the code in the message
        };

        return JsonConvert.SerializeObject(message);
    }
}