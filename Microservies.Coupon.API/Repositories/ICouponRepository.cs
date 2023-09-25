using Microservices.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microservices.CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<List<CouponDto>> GetCouponsAsync();
        Task<CouponDto?> GetCouponByIdAsync(int couponId);
        Task<bool> UpsertCouponAsync(CouponDto couponDto);
        Task<bool> DeleteCouponAsync(int couponId);
        Task<CouponDto?> GetCouponByCodeAsync(string couponCode);
    }
}
