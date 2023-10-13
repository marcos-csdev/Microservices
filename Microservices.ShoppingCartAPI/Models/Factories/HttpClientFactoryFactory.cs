using Microservices.ShoppingCartAPI.Utility;
using System.Runtime.InteropServices;

namespace Microservices.ShoppingCartAPI.Models.Factories
{
    public static class HttpClientFactoryFactory
    {
        public static HttpClient Create(IHttpContextAccessor contextAccessor)
        {
            return HttpClientFactory.Create(new AuthenticationHandler(contextAccessor));
        }
    }
}
