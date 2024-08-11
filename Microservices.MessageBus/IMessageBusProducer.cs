public interface IMessageBusProducer
{
    void PublishMessage<TMessage>(TMessage message, string queueName, string exchangeName);
}