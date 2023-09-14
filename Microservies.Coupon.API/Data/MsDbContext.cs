using Microsoft.EntityFrameworkCore;
using Microservices.CouponAPI.Models;

namespace Microservices.CouponAPI.Data
{
    public class MsDbContext : DbContext
    {
        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        public DbSet<CouponModel> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //without this command migrations will throw a primary key error 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CouponModel>().HasData(
                new CouponModel()
                {
                    Id = 1,
                    CouponCode = "10OFF",
                    Discount = 10,
                    MinExpense = 20,
                });

            modelBuilder.Entity<CouponModel>().HasData(
                new CouponModel()
                {
                    Id = 2,
                    CouponCode = "20OFF",
                    Discount = 20,
                    MinExpense = 40,
                });
            modelBuilder.Entity<CouponModel>().HasData(
                new CouponModel()
                {
                    Id = 3,
                    CouponCode = "30OFF",
                    Discount = 30,
                    MinExpense = 90,
                });
        }
    }
}
