namespace Microservices.ShoppingCartAPI.Services
{
    public class MessageBusService
    {
        private static string _connectionString = null!;
        public MessageBusService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /*public static async Task PublishMessage<T>(T message, string )
        {

        }*/

    }
}
