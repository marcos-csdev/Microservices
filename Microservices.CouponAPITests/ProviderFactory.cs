﻿using AutoMapper;
using FluentAssertions.Common;
using Microservices.Web.Services;
using Microservices.Web.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CouponAPITests
{
    public class ProviderFactory
    {
        public static ICouponService SetCouponServiceProvider(IServiceScope scope)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient<ICouponService, CouponService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();

            scope = serviceCollection.BuildServiceProvider().CreateScope();

            var service = scope.ServiceProvider.GetService<ICouponService>();

            return service!;
        }


        
    }
}
