using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microservices.Coupon.Web.Tests.Services
{
    public class CouponServiceTests : CustomWebApplicationFactory<ICouponService>
    {
        protected override ICouponService SetServiceProvider(IServiceScope scope)
        {
            return ServiceProviderFactory.SetCouponServiceProvider(Scope!);
        }

        public CouponServiceTests() : base()
        {
        }

        [Fact]
        public async Task GetAllEntitiesAsync_Coupons_Found()
        {
            // Arrange
            if (Service is null)
            {
                Assert.Fail("There was a problem setting up the CouponService provider");
            }

            //Act
            var coupons = await Service.GetAllCouponsAsync();

            //Assert
            coupons?.IsSuccess.Should().BeTrue();

            coupons?.Result?.Should().NotBeNull();


        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetEntityByIdAsyncTest(int id)
        {
            // Arrange
            if (Service is null)
            {
                Assert.Fail("There was a problem setting up the CouponService provider");
            }

            //Act
            var coupon = await Service.GetCouponByIdAsync(id);

            //Assert
            coupon?.IsSuccess.Should().BeTrue();
            coupon?.Result.Should().NotBeNull();

            var content = (JObject) coupon?.Result!;
            content.Root.Children().Count().Should().BeGreaterThan(0);
        }
    }
}