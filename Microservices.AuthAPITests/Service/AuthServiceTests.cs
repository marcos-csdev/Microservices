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
        private static LoginRequestDto _loginUser = null!;

        public AuthServiceTests() : base()
        {
            _registrationUser = new RegistrationRequestDto
            {
                Email = "test@email.com",
                Name = "Test",
                Password = "P@ssw0rd",
                Phone = "555-555-5555"
            };

            _loginUser = new LoginRequestDto
            {
                Password = _registrationUser.Password,
                UserName = _registrationUser.Email
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
        public async Task Login_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var createdUser = await Service.Login(_loginUser);

            if(createdUser is null) Assert.Fail("User is null");

            createdUser.User.Should().Be(_loginUser.UserName);

        }

        [Fact]
        public async Task RemoveUser_Removal_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var isSuccessful = await Service.RemoveUser(_registrationUser.Email);
            isSuccessful.Should().BeTrue();


        }

        [Fact]
        public async Task W_Register_User_Registration_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var createdUser = await Service.Register(_registrationUser);

            createdUser.Should().BeEmpty();

        }

        [Fact]
        public async Task Y_AssignRole()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var roleName = "admin"; 

            var isRoleAssigned = await Service.AssignRole(_registrationUser.Email, roleName);
            isRoleAssigned.Should().BeTrue();
        }
        [Fact]
        public async Task Z_RemoveUser_Removal_Successful()
        {
            if (Service is null) Assert.Fail("There was a problem setting up the Auth Service");

            var isSuccessful = await Service.RemoveUser(_registrationUser.Email);
            isSuccessful.Should().BeTrue();


        }

    }
}