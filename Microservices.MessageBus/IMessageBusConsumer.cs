public interface IMessageBusConsumer
{
    string ConsumeMessage<TMessage>(string queueName, string exchangeName, string routingKey));
}