using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Microservices.MessageBus
{
    public class RabbitMQMB : IRabbitMQMB
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly IConnection _connection;

        public RabbitMQMB()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";

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
            //creates queue and binds it to the channel
            channel.QueueDeclare(queueName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            //publishes message to the queue
            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);

            //queue is also removed with the channel
            channel.Dispose();
        }
    }
}
