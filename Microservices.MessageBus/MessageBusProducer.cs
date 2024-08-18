
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Microservices.MessageBus;
public class MessageBusProducer : RabbitMQConfig, IMessageBusProducer
{
    public string QueueName { get; set; } = "";
    public string ExchangeName { get; set; } = "";

    public void PublishMessage<TMessage>(TMessage message, string queueName, string exchangeName)
    {
        using var channel = _connection.CreateModel();
        if (channel == null) return;

        QueueName = queueName;
        ExchangeName = exchangeName;

        try
        {
            //checks is queue exists, if not, creates it
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

            var body = SerializeMessage(message);

            //publishes message to the queue
            channel.BasicPublish("", QueueName, null, body);
        }
        catch (Exception)
        {
            throw;
        }

    }

    private static byte[] SerializeMessage<TMessage>(TMessage message)
    {
        var json = JsonConvert.SerializeObject(message);

        if (json == null) throw new Exception("Error serializing message into JSON");

        var body = Encoding.UTF8.GetBytes(json);
        if (body == null) throw new Exception("Error encoding message into bytes");

        return body;
    }

}