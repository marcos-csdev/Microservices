using Microservices.CouponAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CouponAPITests
{
    public class TestSetup : IDisposable
    {
        protected readonly HttpClient? HttpClient;

        protected TestSetup(IServiceCollection serviceDescriptors)
        {

            try
            {
                var webApplicationFactory = new WebApplicationFactory<Program>();

                HttpClient = webApplicationFactory.WithWebHostBuilder(whb =>
                {
                    var connectionString = "";
                    whb.ConfigureServices((context, services) =>
                    {
                        services.RemoveAll(typeof(MsDbContext));

                        connectionString =          context.Configuration.GetConnectionString("DefaultConnection");

                        services.AddDbContext<MsDbContext>(option =>
                            option.UseSqlServer(connectionString));

                        services.AddLogging(configure =>
                            configure.AddSerilog());
                    });

                    whb.ConfigureTestServices(services =>
                    {
                        //Add specific service here
                        services.Add(serviceDescriptors);
                    });
                }).CreateClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Exception message: {ex.Message}");
            }
        }

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
