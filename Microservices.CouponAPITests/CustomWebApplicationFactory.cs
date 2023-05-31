using Microservices.CouponAPI.Data;
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

namespace Microservices.CouponAPITests
{
    public abstract class CustomWebApplicationFactory<TServiceInterface> : WebApplicationFactory<Web.Program>
    {
        protected HttpClient? HttpClient { get; private set; }
        protected IServiceScope? Scope = null!;
        protected TServiceInterface? Service;

        public CustomWebApplicationFactory()
        {

            HttpClient = WithWebHostBuilder(whb =>
            {
                whb.ConfigureServices((context, services) =>
                {
                    ConfigureServices(context, services);
                });

                whb.ConfigureTestServices(services =>
                {
                    Service = SetServiceProvider(Scope);
                });
            }).CreateClient();

        }

        private void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services.RemoveAll(typeof(MsDbContext));

            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MsDbContext>(option =>
                option.UseSqlServer(connectionString));

            services.AddLogging(configure =>
                configure.AddSerilog());

            Scope = services.BuildServiceProvider().CreateScope();


        }

        protected abstract TServiceInterface SetServiceProvider(IServiceScope scope);


        protected new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
