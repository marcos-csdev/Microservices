using Microservices.EmailAPI.Services;
using Microservices.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Microservices.EmailAPI.Messaging;


public class BackgroundSvcConsumer : BackgroundService, IDisposable
{
    private IConnection? _connection;
    private IModel? _channel;
    private IServiceScopeFactory _serviceScopeFactory;

    public BackgroundSvcConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        SetupRabbitMQ();
    }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
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
            VirtualHost = MessageBusConfig.VirtualHost!,
            Port = MessageBusConfig.Port!,

        }.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare(MessageBusConfig.QueueName, false, false, false, null);
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
        consumer.Received += (sender, eventArgs) =>
        {
            var msgBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var email = JsonConvert.DeserializeObject<string>(msgBody);

            HandleMessage(email!).GetAwaiter().GetResult();

            //return msg to message bus acknowledging that the message was processed successfully
            _channel!.BasicAck(eventArgs.DeliveryTag, false);
        };

        //assigning handler to channel
        _channel.BasicConsume(MessageBusConfig.QueueName, false, consumer);

        return Task.CompletedTask;
    }

    private async Task HandleMessage(string email)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            //here IEmailService is called through service provider because of how it is called on Program.cs (AddHostedService)
            var emailService = scope.ServiceProvider.GetService<IEmailService>();

            await emailService!.RegisterUserEmail(email);
        }

    }
}

