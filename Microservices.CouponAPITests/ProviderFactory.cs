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

        public static IServiceProvider SetService<TClass>(IServiceCollection serviceCollection) where TClass : class
        {
            serviceCollection.AddSingleton<TClass>();
            _serviceScope = serviceCollection.BuildServiceProvider().CreateScope();
            var serviceProvider = _serviceScope.ServiceProvider.GetRequiredService<TClass>();

            return (IServiceProvider)serviceProvider;

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
