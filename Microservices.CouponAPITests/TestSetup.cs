using Microservices.CouponAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Microservices.CouponAPITests
{
    public abstract class TestSetup : IDisposable
    {
        public HttpClient? HttpClient { get; private set; }

        public void SetupService<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {

            try
            {
                var webApplicationFactory = new WebApplicationFactory<Web.Program>();

                HttpClient = webApplicationFactory.WithWebHostBuilder(whb =>
                {
                    whb.ConfigureServices((context, services) =>
                    {
                        services.RemoveAll(typeof(MsDbContext));

                        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                        services.AddDbContext<MsDbContext>(option =>
                            option.UseSqlServer(connectionString));

                        services.AddLogging(configure =>
                            configure.AddSerilog());


                    });

                    whb.ConfigureTestServices((services) =>
                    {
                        SetServiceDIContainer<TInterface, TService>();

                        //ConfigureTestServices<TInterface, TService>(services);
                    });
                }).CreateClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Exception message: {ex.Message}");
            }
        }


        protected abstract TService SetServiceDIContainer<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface;
        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) HttpClient?.Dispose();
        }

    }
}
