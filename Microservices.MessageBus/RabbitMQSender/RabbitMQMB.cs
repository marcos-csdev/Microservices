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
        private string _hostName;
        private string _userName;
        private string _password;
        private IConnection _connection;

        public RabbitMQMB()
        {
            if (_connection == null)
                CreateConnection();

        }


        private void CreateConnection()
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

                //creates queue (if it does not exist) and binds it to the channel
                channel.QueueDeclarePassive(queueName);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                //publishes message to the queue
                channel.BasicPublish("", queueName, null, body);

            }
            catch (Exception )
            {
                throw;
            }

        }
    }
}
