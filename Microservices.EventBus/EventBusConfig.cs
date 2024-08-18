

namespace Microservices.EventBus;

public static class EventBusConfig
{
    public static string Host { get; set; } = null!;
    public static string UserName { get; set; } = null!;
    public static string Password { get; set; } = null!;
    public static string QueueName { get; set; } = null!;
    public static string ExchangeName { get; set; } = null!;
    public static int Port { get; set; } = 0;
    public static string VirtualHost { get; set; } = null!;
}

