using Microservices.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.ProductAPI.Data
{
    public class MsDbContext : DbContext
    {
        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //without this command migrations will throw a primary key error 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 1,
                Name = "Samosa",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/603x403",
                Category = "Appetizer"
            });
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 2,
                Name = "Paneer Tikka",
                Price = 13.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/602x402",
                Category = "Appetizer"
            });
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 3,
                Name = "Sweet Pie",
                Price = 10.99,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/601x401",
                Category = "Dessert"
            });
            modelBuilder.Entity<ProductModel>().HasData(new ProductModel
            {
                Id = 4,
                Name = "Pav Bhaji",
                Price = 15,
                Description = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                ImageUrl = "https://placehold.co/600x400",
                Category = "Entree"
            });
        }
    }
}
