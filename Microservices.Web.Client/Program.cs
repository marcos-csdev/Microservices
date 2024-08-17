using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Text;
namespace Microservices.Web.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            // Add HttpClient, set API Base URLs and add corresponding service wrappers
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<IProductService, ProductService>();
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddHttpClient<ICartService, CartService>();
            builder.Services.AddHttpClient<IAuthService, AuthService>();

            StaticDetails.CouponAPIUrl = builder.Configuration["ServiceUrls:CouponAPI"]!;
            StaticDetails.ProductAPIUrl = builder.Configuration["ServiceUrls:ProductAPI"]!;
            StaticDetails.AuthAPIUrl = builder.Configuration["ServiceUrls:AuthAPI"]!;
            StaticDetails.CartAPIUrl = builder.Configuration["ServiceUrls:ShoppingCartAPI"]!;

            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            AddAuthentication(builder);

            AddSeriLog(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        static void AddSeriLog(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((fileContext, loggingConfig) =>
            {
                var filePath = Path.Join("logs", "log.log");
                loggingConfig.WriteTo.File(filePath, rollingInterval: RollingInterval.Day);
            });
        }

        static void AddAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(10);
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });
        }
    }
}
