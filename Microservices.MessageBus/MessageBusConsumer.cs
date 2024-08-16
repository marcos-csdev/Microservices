using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservices.MessageBus;
public class MessageBusConsumer : RabbitMQConfig, IMessageBusConsumer
{
    public EventingBasicConsumer Consumer { get; private set; } = null!;

    /// <summary>
    /// Consumes a message from the specified queue using the provided exchange name and routing key.
    /// </summary>
    /// <typeparam name="TMessage">The type of message being consumed.</typeparam>
    /// <param name="queueName">The name of the queue to consume from.</param>
    /// <param name="exchangeName">The name of the exchange to use.</param>
    /// <param name="routingKey">The routing key to use for message consumption.</param>
    /// <returns>The consumed message.</returns>
    public string ConsumeMessage<TMessage>(string queueName, string exchangeName, string routingKey)
    {
        using var channel = _connection.CreateModel();
        BindQueue(queueName, exchangeName, routingKey, channel);

        var subscribedConsumer = SubscribeConsumer(channel);

        var message = channel.BasicConsume(queueName, true, Consumer);

        return message;
    }

    private string SubscribeConsumer(IModel channel)
    {
        //subscribing to the queue 
        //so it can be notified when a message has been sent by a producer (observable)
        Consumer = new EventingBasicConsumer(channel);
        var message = "";

        //The Received event is triggered when a message arrives, notifying the subscribed consumers (observers) 
        Consumer.Received += (sender, args) =>
        {
            var messageBodyBytes = args.Body.ToArray();
            message = Encoding.UTF8.GetString(messageBodyBytes);
        };

        return message;
    }

    private static void BindQueue(string queueName, string exchangeName, string routingKey, IModel channel)
    {
        //creates queue (if it does not exist) and binds it to the channel
        channel.QueueDeclare(queueName);

        //exchange and queue names are needed to set up the binding
        channel.QueueBind(queueName, exchangeName, routingKey);
    }
}