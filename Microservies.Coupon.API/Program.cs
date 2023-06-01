using Microservices.CouponAPI;
using Microservices.CouponAPI.Data;
using Microservices.CouponAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Microservices.CouponAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //=================Adding DbContext========================
            builder.Services.AddDbContext<MsDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //=================Adding DbContext========================

            //=================Adding AutoMapper========================
            var mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //=================Adding AutoMapper========================

            builder.Services.AddScoped<ICouponRepository, CouponRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //=================Adding Serilog========================
            builder.Host.UseSerilog((fileContext, loggingConfig) =>
            {
                loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                loggingConfig.MinimumLevel.Debug();
            });
            //=================Adding Serilog========================

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            ApplyPendingMigrations();

            app.Run();


            //applies the update-database command when the project runs
            void ApplyPendingMigrations()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<MsDbContext>();

                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();

                    }
                }
            }
        }
    }
}