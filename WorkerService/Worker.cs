using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using EmailWorkerService.Service;
using DrinksProject.Models.EmailModels;

namespace EmailWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        private readonly IEmailSender _emailSender;
        private IConnection _connection;
        private IModel _channel;

        public Worker(IConfiguration configuration, ILogger<Worker> logger, IEmailSender emailSender)
        {
            _hostname = configuration["RabbitMQ:Hostname"] ??
                throw new ArgumentNullException("RabbitMQ:Hostname", "RabbitMQ:Hostname configuration is missing.");
            _queueName = configuration["RabbitMQ:QueueName"] ??
                throw new ArgumentNullException("RabbitMQ:QueueName", "RabbitMQ:QueueName configuration is missing.");
            _username = configuration["RabbitMQ:Username"] ??
                throw new ArgumentNullException("RabbitMQ:Username", "RabbitMQ:Username configuration is missing.");
            _password = configuration["RabbitMQ:Password"] ??
                throw new ArgumentNullException("RabbitMQ:Password", "RabbitMQ:Password configuration is missing.");

            _logger = logger;
            _emailSender = emailSender;

            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };

                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                _connection = connection;
                _channel = channel;
            }
            catch (Exception ex)
            {
                string message = $"Error initializing RabbitMQ listener: {ex.Message}";
                _logger.LogError(message);
                throw;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<EmailConfirmationMessage>(content);

                if (message != null)
                {
                    _logger.LogInformation($"Received message: {content}"); // Add this line

                    _emailSender.SendEmail(message.ToEmail, message.Subject, message.Body);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();

            base.Dispose();
        }
    }
}