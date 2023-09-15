using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class CouponService : BaseService, ICouponService
    {
        public CouponService(IMessageService messageService) : base(messageService)
        {
        }

        public async Task<ResponseDto?> AddCouponAsync(CouponDto couponDto) 
        {
            return await AddEntityAsync(couponDto, $"{StaticDetails.CouponAPIUrl}/api/coupons/create");

        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await GetAllEntitiesAsync($"{StaticDetails.CouponAPIUrl}/api/coupons");

        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await GetEntityByIdAsync(id,
                $"{StaticDetails.CouponAPIUrl}/api/coupons/GetById/{id}");
        }


        public async Task<ResponseDto?> RemoveCouponAsync(int id)
        {
            return await RemoveEntityAsync(id,
                $"{StaticDetails.CouponAPIUrl}/api/coupons/remove/{id}");
        }

        public async Task<ResponseDto?> UpdateCouponAsync(string id, CouponDto couponDto)
        {
            return await UpdateEntityAsync(id, couponDto,
                $"{StaticDetails.CouponAPIUrl}/api/coupons/update");

        }

    }
}
