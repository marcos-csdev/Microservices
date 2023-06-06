using FluentAssertions;
using FluentAssertions.Common;
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
    public class AuthServiceTests : CustomWebApplicationFactory<IAuthService>
    {

        private static RegistrationRequestDto _registrationUser = null!;

        public AuthServiceTests() : base()
        {
            _registrationUser = new RegistrationRequestDto
            {
                Email = "test@email.com",
                Name = "Test",
                Password = "P@ssw0rd",
                Phone = "555-555-5555"
            };

        }

        protected override IAuthService SetServiceProvider(IServiceCollection services, IServiceScope scope)
        {

            return ServiceProviderFactory.SetAuthServiceProvider(services, scope);
        }

        [Fact]
        public async Task Registration_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var createdUser = await Service.Register(_registrationUser);

            createdUser.Should().BeEmpty();

        }
        

        [Fact]
        public async Task RemoveUser_Removal_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var isSuccessful = await Service.RemoveUser(_registrationUser.Email);
            isSuccessful.Should().BeTrue();


        }


    }
}