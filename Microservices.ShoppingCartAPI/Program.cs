using MassTransit;
using Microservices.MessageBus;
using Microservices.ShoppingCartAPI.Data;
using Microservices.ShoppingCartAPI.Extensions;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Services;
using Microservices.ShoppingCartAPI.Services.Abstractions;
using Microservices.ShoppingCartAPI.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using Serilog;

namespace Microservices.ShoppingCartAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddDbContext(builder);

            AddAutoMapper(builder);
            AddMessageBus(builder);

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services
                .AddScoped<AuthenticationHandler>();

            builder.Services.AddScoped<IMessageBusProducer, MessageBusProducer>();

            SetAPIsUrls(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.AddSwaggerGenConfigExtension();

            builder.AutenticateJwtTokenExtension();

            AddSeriLog(builder);

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
                using var scope = app.Services.CreateScope();
                var _db = scope.ServiceProvider.GetRequiredService<MsDbContext>();

                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();

                }
            }

            void AddSeriLog(WebApplicationBuilder builder)
            {
                builder.Host.UseSerilog((fileContext, loggingConfig) =>
                {
                    var filePath = Path.Join("logs", "log.log");
                    loggingConfig.WriteTo.File(filePath, rollingInterval: RollingInterval.Day);

                });

            }

            void AddAutoMapper(WebApplicationBuilder builder)
            {
                var mapper = MappingConfig.RegisterMaps().CreateMapper();
                builder.Services.AddSingleton(mapper);
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            }

            void AddDbContext(WebApplicationBuilder builder)
            {
                builder.Services.AddDbContext<MsDbContext>(option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }

            void SetAPIsUrls(WebApplicationBuilder builder)
            {

                var productAPIUrl = builder.Configuration["ServiceUrls:ProductAPI"]!;
                StaticDetails.ProductAPIUrl = productAPIUrl;


                builder.Services.AddHttpClient("Product",
                    url => url.BaseAddress = new Uri(productAPIUrl!));

                var couponAPIUrl = builder.Configuration["ServiceUrls:CouponAPI"]!;
                StaticDetails.CouponAPIUrl = couponAPIUrl;

                //builder.Services.AddHttpClient("Coupon",
                //    uri => uri.BaseAddress = new Uri(couponAPIUrl))
                //    .AddHttpMessageHandler<AuthenticationHandler>();

            }


        }

        private static void AddMessageBus(WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(builder.Configuration["MessageBusSettings:HostAddress"], "/", host =>
                    {
                        host.Username(builder.Configuration["MessageBusSettings:UserName"]);
                        host.Password(builder.Configuration["MessageBusSettings:Password"]);
                    });
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}