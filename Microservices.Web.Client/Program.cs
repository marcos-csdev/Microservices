using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Serilog;
using System.Text;
namespace Microservices.Web.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"]!;
            StaticDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"]!;
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddHttpClient<IAuthService, AuthService>();

            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            //=================Adding Serilog========================
            builder.Host.UseSerilog((fileContext, loggingConfig) =>
            {
                loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                loggingConfig.MinimumLevel.Debug();
            });
            //=================Adding Serilog========================


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
