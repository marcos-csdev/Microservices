using Microservices.EmailAPI.Services;
using Microservices.EmailAPI.Utility;
using Microservices.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Microservices.EmailAPI.Messaging
{
    /// <summary>
    /// BackgroundService automatically starts the service
    /// </summary>
    public class RabbitMQConsumer : BackgroundService, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;
        private string? _queueName;
        private IServiceScopeFactory _serviceScopeFactory;


        public RabbitMQConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            SetupRabbitMQ();
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
        }
        

        private void SetupRabbitMQ()
        {
            _connection = new ConnectionFactory
            {
                HostName = MessageBusConfig.Host!,
                UserName = MessageBusConfig.UserName!,
                Password = MessageBusConfig.Password!,
            }.CreateConnection();

            _channel = _connection.CreateModel();
            _queueName = MessageBusConfig.QueueName;

            _channel.QueueDeclare(_queueName, false, false, false, null);
        }
        public async Task RunAsync(CancellationToken stoppingToken)
        {
            await ExecuteAsync(stoppingToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //cancellation token stops the pipeline right away
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (chnl, eventArgs) =>
            {
                var msgBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var email = JsonConvert.DeserializeObject<string>(msgBody);

                HandleMessage(email!).GetAwaiter().GetResult();

                //return msg consuming ack signal 
                _channel!.BasicAck(eventArgs.DeliveryTag, false);
            };

            //assigning handler to channel
            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string email)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetService<IEmailService>();

                await emailService!.RegisterUserEmail(email);
            }

        }
    }
}
