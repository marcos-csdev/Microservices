using Microservices.CouponAPI.Data;
using Microservices.Web;
using Microservices.Web.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Microservices.Coupon.Web.Tests
{
    public abstract class CustomWebApplicationFactory<TServiceInterface> : WebApplicationFactory<Program> where TServiceInterface : class
    {
        protected HttpClient? HttpClient { get; private set; }
        protected IServiceScope? Scope = null!;
        protected TServiceInterface? Service;

        public CustomWebApplicationFactory()
        {

            HttpClient = WithWebHostBuilder(whb =>
            {

                whb.ConfigureTestServices(services =>
                {
                    Scope = services.BuildServiceProvider().CreateScope();
                    Service = SetServiceProvider(Scope);
                });
            }).CreateClient();

        }


        protected abstract TServiceInterface SetServiceProvider(IServiceScope scope);


        protected new void Dispose()
        {
            Dispose(true);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Scope?.Dispose();

                HttpClient?.Dispose();
            }

            // The disposal here is done, so let the parent do the same
            base.Dispose(disposing);
        }
    }
}
