using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.MessageBus
{
    public interface IMessageBusConfig
    {
        static string Host { get; set; } = null!;
        static string UserName { get; set; } = null!;
        static string Password { get; set; } = null!;    
        static string QueueName { get; set; } = null!;
    }
}
