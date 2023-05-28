using Microsoft.Extensions.DependencyInjection;
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


        public void Dispose()
        {
            if (_serviceScope is not null)
            {
                _serviceScope.Dispose();
            }
        }

        public static IServiceProvider GetServiceProvider<TInterface, TService>( IServiceCollection serviceCollection)
            where TInterface : class
            where TService : class, TInterface
        {
            serviceCollection.AddTransient<TInterface, TService>();
            _serviceScope = serviceCollection.BuildServiceProvider().CreateScope();
            var serviceProvider = _serviceScope.ServiceProvider.GetRequiredService<TInterface>();

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

        /*private static IServiceCollection GetSomeService() where TService : class
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<TService>();
            //ReplaceDbContext(serviceCollection, newConnectionString);
            var scope = serviceCollection.BuildServiceProvider().CreateScope();
            var testInstance = scope.ServiceProvider.GetService<TTestType>();

            if (testInstance == null) return;

            SetTestInstance(testInstance);

        }*/
    }
}
