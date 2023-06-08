using Microservices.Web.Services;
using Microservices.Web.Services.Abstractions;
using Microservices.Web.Utility;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Text;

namespace Microservices.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddHttpClient<IAuthService, AuthService>();


            StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"]!;
            StaticDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"]!;

            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            ////repo
            //builder.Services.AddControllersWithViews();
            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddHttpClient();
            //builder.Services.AddHttpClient<ICouponService, CouponService>();
            //builder.Services.AddHttpClient<IAuthService, AuthService>();

            //StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"]!;
            //StaticDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"]!;
            //builder.Services.AddScoped<IBaseService, BaseService>();
            //builder.Services.AddScoped<IAuthService, AuthService>();
            //builder.Services.AddScoped<ICouponService, CouponService>();

            ////repo

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


            app.Use(async (context, next) =>
            {
                var body = "";//= GetRawBodyString(context, encoding: Encoding.UTF8);

                using (var content = new StreamContent(context.Request.Body))
                {
                    body = await content.ReadAsStringAsync();
                }


                if (context.Response.StatusCode == 404)
                {
                    
                    context.Request.Path = "/Home";
                    await next();
                }
            });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        public static string GetRawBodyString(HttpContext httpContext, Encoding encoding)
        {
            var body = "";
            httpContext.Request.EnableBuffering();
            if (httpContext.Request.ContentLength == null || !(httpContext.Request.ContentLength > 0) ||
                !httpContext.Request.Body.CanSeek) return body;
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(httpContext.Request.Body, encoding, true, 1024, true))
            {
                body = reader.ReadToEnd();
            }
            httpContext.Request.Body.Position = 0;
            return body;
        }
    }
}
