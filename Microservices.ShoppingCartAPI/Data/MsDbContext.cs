
using Microservices.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.ShoppingCartAPI.Data
{
    public class MsDbContext : DbContext
    {
        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeaderModel> CartHeaders { get; set; }
        public DbSet<CartDetailsModel> CartDetails { get; set; }

    }
}
