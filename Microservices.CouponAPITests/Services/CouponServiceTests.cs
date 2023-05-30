using Microservices.Web.Models;
using Microservices.Web.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace Microservices.CouponAPITests.Services
{
    public class CouponServiceTests : CustomWebApplicationFactory<ICouponService>
    {
        private readonly ICouponService _couponService;

        protected override ICouponService SetServiceProvider(IServiceScope scope)
        {
            return ProviderFactory.SetCouponServiceProvider(Scope!);
        }

        public CouponServiceTests() : base()
        {
            _couponService = SetServiceProvider(Scope!);
        }

        [Fact]
        public async Task GetAllEntitiesAsync_Coupons_Found()
        {
            // Arrange

            var coupons = await _couponService.GetAllEntitiesAsync<ResponseDto>();

            coupons?.IsSuccess.Should().BeTrue();

            coupons?.Result?.Should().NotBeNull();


        }
    }
}