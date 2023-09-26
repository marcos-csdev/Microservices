using System.Runtime.InteropServices;

namespace Microservices.Web.Client.Models.Factories
{
    public static class CartHeaderDtoFactory
    {
        public static CartHeaderDto Create(int cartHeaderId, double total, 
            string couponCode, double discount, string userId)
        {
            return new CartHeaderDto
            {
                CartHeaderId = cartHeaderId,
                CartTotal = total,
                CouponCode = couponCode,
                Discount = discount,
                UserId = userId
            };
        }
        public static CartHeaderDto Create(string userId)
        {
            return new CartHeaderDto
            {
                UserId = userId
            };
        }
    }
}
