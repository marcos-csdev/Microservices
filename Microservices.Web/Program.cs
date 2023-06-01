using Microservices.Web.Services;
using Microservices.Web.Services.Abstractions;
using Microservices.Web.Utility;
using Serilog;

namespace Microservices.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //CouponAPIKey is stored at a user secret file
            StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPIUrl"]!;

            // Add services to the container.

            //=================Adding Serilog========================
            builder.Host.UseSerilog((fileContext, loggingConfig) =>
            {
                loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                loggingConfig.MinimumLevel.Debug();
            });
            //=================Adding Serilog========================
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddScoped<ICouponService, CouponService>();

            builder.Services.AddControllersWithViews();

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
