using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface ICouponService
    {

        Task<ResponseDto?> AddCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> RemoveCouponAsync(int id);
        Task<ResponseDto?> UpdateCouponAsync(string id, CouponDto couponDto);
    }
}
