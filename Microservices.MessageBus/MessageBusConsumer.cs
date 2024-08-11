using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservices.MessageBus;
public class MessageBusConsumer : RabbitMQConfig, IMessageBusConsumer
{
    public string ConsumeMessage<TMessage>(string queueName, string exchangeName, string routingKey)
    {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queueName);

        //exchange and queue names are needed to set up the binding
        channel.QueueBind(queueName, exchangeName, routingKey);

        //subscribing to the queue 
        //so it can be notified when a message has been sent by a producer (observable)
        var consumer = new EventingBasicConsumer(channel);
        string message = "";

        //The Received event is triggered when a message arrives, notifying the subscribed consumers (observers) 
        consumer.Received += (sender, args) =>
        {
            var messageBodyBytes = args.Body.ToArray();
            message = Encoding.UTF8.GetString(messageBodyBytes);
        };

        channel.BasicConsume(queueName, true, consumer);

        return message;
    }
}