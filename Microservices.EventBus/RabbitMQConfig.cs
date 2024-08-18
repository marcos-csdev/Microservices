using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Microservices.EventBus;

public abstract class RabbitMQConfig : IDisposable
{
    protected string _hostName = string.Empty;
    protected string _userName = string.Empty;
    protected string _password = string.Empty;
    protected IConnection _connection = null!;

    public RabbitMQConfig()
    {
        if (_connection == null)
            CreateConnection();

    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    protected void CreateConnection()
    {
        //reads from client app appsettings.json
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();


        _hostName = config.GetSection("RabbitMQLogin:host").Value!;
        _password = config.GetSection("RabbitMQLogin:password").Value!;
        _userName = config.GetSection("RabbitMQLogin:user").Value!;

        _connection = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        }.CreateConnection();
    }


}

