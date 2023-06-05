using FluentAssertions;
using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service;
using Microservices.AuthAPI.Service.Abstractions;
using Microservices.AuthAPI.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microservices.AuthAPI.Service.Tests
{
    public class AuthServiceTests:CustomWebApplicationFactory<IAuthService> 
    {

        protected override IAuthService SetServiceProvider(IServiceScope scope)
        {
            return ServiceProviderFactory.SetAuthServiceProvider(scope);
        }


        /*public void LoginTest()
        {
            throw new NotImplementedException();
        }*/

        [Fact]
        public async void RegisterTest_Registration_Successful()
        {
            if(Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var user = new RegistrationRequestDto
            {
                Email = "test@email.com",
                Name = "Test",
                Password = "P@ssw0rd",
                Phone = "555-555-5555"
            };

            var createdUser = await Service.Register(user);

            createdUser.Should().BeEmpty();



            var isUserRemoved = await RemoveUser(user.Email);

            isUserRemoved.Should().NotBe(EntityState.Unchanged);

        }

        private async Task<EntityState> RemoveUser(string email)
        {
            if (Service is null) return EntityState.Unchanged;

            var userRemoved = await Service.RemoveUser(email);

            return EntityState.Deleted;
        }

    }
}