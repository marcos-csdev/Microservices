using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CouponAPITests
{
    public class ProviderFactory : IDisposable
    {
        private static IServiceScope? _serviceScope = null!;

        public static IServiceProvider GetServiceProvider<TInterface, TService>(IServiceCollection serviceCollection)
        where TInterface : class
        where TService : class, TInterface
        {

            /*var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SourceMappingProfile());
            });
            mappingConfig.CreateMapper();*/
            serviceCollection.AddAutoMapper(typeof(Program));

            serviceCollection.AddTransient<TInterface, TService>();
            _serviceScope = serviceCollection.BuildServiceProvider().CreateScope();
            var serviceProvider = _serviceScope.ServiceProvider.GetRequiredService<TService>();

            return (IServiceProvider)serviceProvider;

        }

        public static IServiceCollection SetDefaultServiceDIContainer<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<TInterface, TService>();

            return serviceCollection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _serviceScope?.Dispose();
        }
    }
}
