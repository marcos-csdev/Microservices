using AutoMapper;
using MassTransit;
using Microservices.AuthAPI;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Repositories;
using Microservices.AuthAPI.Service;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microservices.MessageBus;
using Serilog;

namespace Microservices.AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddDbContext(builder);

            ConfigureToken(builder);

            AddAutoMapper(builder);

            AddIdentity(builder);

            //=================Adding Services========================
            builder.Services.AddScoped<IUserManagerService, UserManagerService>();
            builder.Services.AddScoped<IRoleManagerService, RoleManagerService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            AddMassTransit(builder);

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IMessageBusProducer, MessageBusProducer>();

            //=================Adding Services========================

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            AddSeriLog(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Authentication must come before authorization, it might not work otherwise
            app.UseAuthentication();

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
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                var mapper = mapperConfig.CreateMapper();
                builder.Services.AddSingleton(mapper);
            }

            void AddDbContext(WebApplicationBuilder builder)
            {
                builder.Services.AddDbContext<MsDbContext>(option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }

            void AddIdentity(WebApplicationBuilder builder)
            {
                builder.Services.AddIdentity<MSUser, IdentityRole>(
                auth => auth.Password.RequireNonAlphanumeric = false)
                .AddEntityFrameworkStores<MsDbContext>()
                .AddDefaultTokenProviders();
            }
        }

        private static void AddMassTransit(WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(options =>
            {
                options.UsingRabbitMq((context, config) =>
                {
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureToken(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("ApiSettings:JwtOptions"));
        }
    }
}
