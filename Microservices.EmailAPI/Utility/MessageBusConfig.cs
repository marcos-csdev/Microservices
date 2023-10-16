using Microservices.MessageBus;

namespace Microservices.EmailAPI.Utility
{
    public class MessageBusConfig : IMessageBusConfig
    {
        public static string Host { get; set; } = null!;
        public static string UserName { get; set; } = null!;
        public static string Password { get; set; } = null!;
        public static string QueueName { get; set; } = null!;
    }
}
