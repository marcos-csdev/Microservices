using Microservices.MessageBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Microservices.MessageBus;
public class MessageBusProducer : RabbitMQConfig, IMessageBusProducer
{

    public void PublishMessage<TMessage>(TMessage message, string queueName, string exchangeName)
    {
        using var channel = _connection.CreateModel();
        if (channel == null) return;

        try
        {
            //check is queue exists, if not, creates it
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            var body = SerializeMessage(message);

            //publishes message to the queue
            channel.BasicPublish("", queueName, null, body);
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

    private static void SetMessageBroker(string queueName, string exchangeName, string routingKey, IModel channel)
    {
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

        //creates queue (if it does not exist) and binds it to the channel
        channel.QueueDeclare(queueName);

        //exchange and queue names are needed to set up the binding
        channel.QueueBind(queueName, exchangeName, routingKey);
    }
}