using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using FluentAssertions.Common;

namespace Microservices.Web.Client.Tests
{
    public class ServiceProviderFactory
    {

        private static readonly MockAuthUser _user = new(
            new Claim("sub", Guid.NewGuid().ToString()),
            new Claim("email", "default-user@xyz.com"));

        public static ICouponService SetCouponServiceProvider(IServiceCollection serviceCollection, IServiceScope scope)
        {


            //public ResponseDto ResponseDto { get; set; } = null!;
            //private readonly IHttpClientFactory _httpClientFactory;
            //private readonly ITokenProvider _tokenProvider;
            serviceCollection.AddScoped(_ => _user);

            serviceCollection.AddHttpClient();//IHttpClientFactory dependency

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();

            var service = scope.ServiceProvider.GetService<ICouponService>();

            return service!;
        }



    }
}
