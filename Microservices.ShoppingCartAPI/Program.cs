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

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services
                .AddScoped<AuthenticationHandler>();
            //builder.Services.ConfigureAll<HttpClientFactoryOptions>(options =>
            //{
            //    options.HttpMessageHandlerBuilderActions.Add(
            //        builder =>
            //        {
            //            builder.AdditionalHandlers.Add(
            //                builder.Services
            //                .GetRequiredService<AuthenticationHandler>());
            //        });
            //});

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
                    loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);

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


                //builder.Services.AddHttpClient("Product",
                //    uri => uri.BaseAddress = new Uri(productAPIUrl))
                //    .AddHttpMessageHandler<AuthenticationHandler>();

                var couponAPIUrl = builder.Configuration["ServiceUrls:CouponAPI"]!;
                StaticDetails.CouponAPIUrl = couponAPIUrl;

                //builder.Services.AddHttpClient("Coupon",
                //    uri => uri.BaseAddress = new Uri(couponAPIUrl))
                //    .AddHttpMessageHandler<AuthenticationHandler>();

            }

        }
    }
}