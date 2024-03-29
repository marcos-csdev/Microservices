﻿using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Services.Abstractions;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class CouponService : BaseService, ICouponService
    {
        public CouponService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public async Task<CouponDto?> GetCoupon(string actionName, string couponCode)
        {
            var url = $"{StaticDetails.CouponAPIUrl}/{actionName}/{couponCode}";
            var response = await SendMessageAsync(url);

            CouponDto? coupon = null!;
            if (response != null && response.IsSuccess)
                coupon = DeserializeResponseToEntity<CouponDto>(response!);

            return coupon;
        }
    }
}
