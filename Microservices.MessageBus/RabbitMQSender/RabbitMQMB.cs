using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration.Json;
using System.Text;
using System.Threading.Channels;

namespace Microservices.MessageBus
{
    public class RabbitMQMB : IRabbitMQMB
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;

        public RabbitMQMB()
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


        public void SendMessage<TMessage>(TMessage message, string queueName)
        {
            using var channel = _connection.CreateModel();
            try
            {

                //creates queue and binds it to the channel
                channel.QueueDeclare(queueName, false, true, true);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                //publishes message to the queue
                channel.BasicPublish("", queueName, null, body);

            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                channel.Dispose();
            }

        }
    }
}
