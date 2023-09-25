using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Services
{
    public interface ICouponService
    {
        Task<CouponDto?> GetCoupon(string couponCode);
    }
}