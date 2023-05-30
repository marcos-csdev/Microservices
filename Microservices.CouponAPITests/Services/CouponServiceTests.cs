
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microservices.Web.Services.Abstractions;
using Microservices.CouponAPITests;
using Microservices.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Microservices.CouponAPITests.Services
{
    public class CouponServiceTests : CustomWebApplicationFactory<ICouponService>
    {

        protected override ICouponService SetServiceProvider(IServiceCollection serviceCollection, IServiceScope scope)
        {
            return ProviderFactory.SetCouponServiceProvider(serviceCollection, scope);
        }

        public CouponServiceTests() : base()
        {

        }
        protected new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        [Test]
        public void GetHomeResource_HttpResponse_ShouldReturn200OK()
        {
            // Arrange


        }
    }
}