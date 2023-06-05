using AutoMapper;
using FluentAssertions.Common;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Repositories;
using Microservices.AuthAPI.Service;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.AuthAPI.Tests
{
    public class ServiceProviderFactory
    {
        public static IAuthService SetAuthServiceProvider(IServiceCollection serviceCollection, IServiceScope scope)
        {
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IAuthService, AuthService>();

            var service = scope.ServiceProvider.GetService<IAuthService>();

            return service!;
        }
        


    }
}
