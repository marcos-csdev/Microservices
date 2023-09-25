using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class CouponService : BaseService, ICouponService
    {
        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<CouponDto?> GetCoupon(string couponCode)
        {
            var url = $"{StaticDetails.CouponAPIURL}/{couponCode}";
            var response = await SendMessageAsync("ShoppingCartAPIClient", url);

            var coupon = DeserializeResponseToEntity<CouponDto>(response!);

            return coupon;
        }
    }
}
