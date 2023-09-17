using AutoMapper;
using FluentAssertions.Common;
using Microservices.CouponAPI.Repositories;
using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Coupon.Web.Tests
{
    public class ServiceProviderFactory
    {
        public static ICouponService SetCouponServiceProvider(IServiceCollection serviceCollection, IServiceScope scope)
        {


            //public ResponseDto ResponseDto { get; set; } = null!;
            //private readonly IHttpClientFactory _httpClientFactory;
            //private readonly ITokenProvider _tokenProvider;

            serviceCollection.AddHttpClient();//IHttpClientFactory dependency
            serviceCollection.AddAuthorization(); 

            serviceCollection.AddTransient<IMessageService, MessageService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();

            var service = scope.ServiceProvider.GetService<ICouponService>();

            return service!;
        }



    }
}
