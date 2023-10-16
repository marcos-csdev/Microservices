using System.Runtime.InteropServices;

namespace Microservices.EmailAPI.Models.Factories
{
    public static class EmailLoggerFactory
    {
        public static EmailLogger Create(string message, string emailAddress)
        {
            return new EmailLogger
            {
                Address = emailAddress,
                Message = message,
                DateSent = DateTime.Now,
            };
        }
    }
}
