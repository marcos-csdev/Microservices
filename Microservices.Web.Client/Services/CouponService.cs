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
            return await AddEntityAsync(couponDto, $"{StaticDetails.CouponAPIUrl}/create");

        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await GetAllEntitiesAsync(StaticDetails.CouponAPIUrl);

        }

        public async Task<ResponseDto?> GetCouponByIdAsync(int id)
        {
            return await GetEntityByIdAsync(
                $"{StaticDetails.CouponAPIUrl}/GetById/{id}");
        }


        public async Task<ResponseDto?> RemoveCouponAsync(int id)
        {
            return await RemoveEntityAsync(
                $"{StaticDetails.CouponAPIUrl}/remove/{id}");
        }

        public async Task<ResponseDto?> UpdateCouponAsync(int id, CouponDto couponDto)
        {
            return await UpdateEntityAsync(couponDto,
                $"{StaticDetails.CouponAPIUrl}/update");

        }

    }
}
