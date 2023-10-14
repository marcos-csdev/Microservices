namespace Microservices.MessageBus
{
    public interface IRabbitMQMB
    {
        void SendMessage<TMessage>(TMessage message, string queueName);
    }
}