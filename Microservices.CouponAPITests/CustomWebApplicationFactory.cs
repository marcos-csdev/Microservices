using Microservices.CouponAPI.Data;
using Microservices.Web.Services.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Microservices.CouponAPITests
{
    public abstract class CustomWebApplicationFactory<TServiceInterface> : WebApplicationFactory<Web.Program>
    {
        private IServiceScope _scope = null!;
        protected HttpClient? HttpClient { get; private set; }

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
                    SetServiceProvider(services, _scope);
                });

            }).CreateClient();
        }

        /*protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
        }*/

        private void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            services.RemoveAll(typeof(MsDbContext));

            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MsDbContext>(option =>
                option.UseSqlServer(connectionString));

            services.AddLogging(configure =>
                configure.AddSerilog());

            _scope = services.BuildServiceProvider().CreateScope();



        }

        protected abstract TServiceInterface SetServiceProvider(IServiceCollection serviceCollection, IServiceScope scope);


        protected new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _scope.Dispose();
        }
    }
}
