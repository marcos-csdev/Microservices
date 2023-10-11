using Microservices.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.EmailAPI.Data
{
    public class MsDbContext : DbContext
    {

        public MsDbContext(DbContextOptions<MsDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //without this command migrations will throw a primary key error 
            base.OnModelCreating(modelBuilder);


        }

    }
}
