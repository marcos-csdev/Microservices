using AutoMapper;
using Microservices.CouponAPI.Data;
using Microservices.CouponAPI.Models;
using Microservices.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microservices.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MsDbContext _dbContext;
        private IMapper _mapper;

        public CouponRepository(MsDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<EntityState> DeleteCouponAsync(int couponId)
        {
            var coupon = await GetDbCouponByIdAsync(couponId);

            if (coupon == null) return EntityState.Unchanged;

            var deletedCoupon = _dbContext.Coupons.Remove(coupon);

            await _dbContext.SaveChangesAsync();

            return deletedCoupon.State;

        }
        private async Task<CouponModel?> GetDbCouponByIdAsync(int couponId)
        {
            if (couponId < 1) return null!;

            var coupon = await _dbContext.Coupons
                .FirstOrDefaultAsync(c => c.Id == couponId);

            return coupon;
        }

        public async Task<CouponDto> GetCouponByIdAsync(int couponId)
        {
            if (couponId < 1) return null!;

            var coupon = await _dbContext.Coupons
                .FirstOrDefaultAsync(c => c.Id == couponId);

            var couponDto = _mapper.Map<CouponDto>(coupon);

            return couponDto;
        }

        public async Task<List<CouponDto>> GetCouponsAsync()
        {
            var dbCoupons = await _dbContext.Coupons.ToListAsync();

            if(dbCoupons == null) return new List<CouponDto>();

            var couponsDto = _mapper.Map<List<CouponDto>>(dbCoupons);

            return couponsDto;
        }

        public async Task<EntityState> UpsertCouponAsync(CouponDto couponDto)
        {
            if(couponDto == null) return EntityState.Unchanged;

            var dbCoupon = _mapper.Map<CouponModel>(couponDto);

            EntityEntry entityEntry = null!;
            //Create
            if(couponDto.Id == 0)
            {
                entityEntry = await _dbContext.Coupons.AddAsync(dbCoupon);
            }
            else//Update
            {
                entityEntry = _dbContext.Coupons.Update(dbCoupon);
            }

            await _dbContext.SaveChangesAsync();

            return entityEntry.State;
        }
    }
}
