using Microservices.Web.Models;
using Microservices.Web.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microservices.CouponAPITests.Services
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
            if(Service is null)
            {
                Assert.Fail("There was a problem setting up the CouponService provider");
            }

            var coupons = await Service.GetAllEntitiesAsync<ResponseDto>();

            coupons?.IsSuccess.Should().BeTrue();

            coupons?.Result?.Should().NotBeNull();


        }
    }
}