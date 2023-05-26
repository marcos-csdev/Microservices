using Microsoft.EntityFrameworkCore;
using Microservies.CouponAPI.Models;

namespace Microservies.CouponAPI.Data
{
    public class MsDbContext : DbContext
    {
        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        DbSet<CouponModel> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CouponModel>().HasData(
                new CouponModel()
                {
                    Id = 1,
                    CouponCode = "10OFF",
                    Discount = 10,
                    MinAmount = 20,
                });

            modelBuilder.Entity<CouponModel>().HasData(
                new CouponModel()
                {
                    Id = 2,
                    CouponCode = "20OFF",
                    Discount = 20,
                    MinAmount = 40,
                });
        }
    }
}
