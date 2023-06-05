using Microservices.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microservices.CouponAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<List<CouponDto>> GetCouponsAsync();
        Task<CouponDto?> GetCouponByIdAsync(int couponId);
        Task<EntityState> UpsertCouponAsync(CouponDto couponDto);
        Task<EntityState> DeleteCouponAsync(int couponId);
    }
}
