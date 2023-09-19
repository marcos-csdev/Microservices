using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class CouponService : BaseService
    {
        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var response = await SendMessageAsync("ShoppingCartAPIClient", $"{StaticDetails.CouponAPIURL}/{couponCode}");

            var coupon = DeserializeResponseToEntity<CouponDto>(response!);

            return coupon;
        }
    }
}
