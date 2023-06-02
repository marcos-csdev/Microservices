using Microservices.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Data
{
    public class MsDbContext : IdentityDbContext<MSUser>
    {
        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        public DbSet<MSUser> MSUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //without this command migrations will throw a primary key error 
            base.OnModelCreating(modelBuilder);

            
        }
    }
}
