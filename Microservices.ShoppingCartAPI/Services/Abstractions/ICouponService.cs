using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Services.Abstractions
{
    public interface ICouponService
    {
        Task<CouponDto?> GetCoupon(string actionName, string couponCode);
    }
}