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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microservices.CouponAPITests
{
    public abstract class TestSetup : IDisposable
    {
        protected readonly HttpClient? HttpClient;

        protected TestSetup(IServiceCollection serviceDescriptors)
        {

            try
            {
                var webApplicationFactory = new WebApplicationFactory<Program>();

                HttpClient = webApplicationFactory.WithWebHostBuilder(whb =>
                {
                    whb.ConfigureServices((context, services) =>
                    {
                        services.AddDbContext<MsDbContext>(option =>
                    option.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    });


                    whb.ConfigureTestServices(sc => sc.Add(serviceDescriptors));
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

        public abstract void AddEntityChange(object newEntity, EntityState entityState);

        private void ReplaceDbContext(IServiceCollection serviceCollection,
                                      string newConnectionString)
        {
            serviceCollection.RemoveAll(typeof(MsDbContext));
            serviceCollection.AddDbContext<MsDbContext>();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing) HttpClient?.Dispose();
        }



        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(
                logging =>
                {

                })
            .ConfigureAppConfiguration;*/




        

        





    }
}
