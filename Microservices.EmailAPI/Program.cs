using Microservices.EmailAPI.Data;
using Microservices.EmailAPI.Extensions;
using Microservices.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microservices.EmailAPI.Messaging;
using Serilog;
using Microservices.EmailAPI.Repositories;
using RabbitMQ.Client;
using Microservices.EmailAPI.Utility;
using Microservices.MessageBus;
using MassTransit;

namespace Microservices.EmailAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddDbContext(builder);

            AddAutoMapper(builder);

            AddMassTransit(builder);

            AssignBusValues(builder);

            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IMessageBusConfig, MessageBusConfig>();

            //automatically starts the consumer
            builder.Services.AddHostedService<RabbitMQConsumer>();

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
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                builder.Services.AddDbContext<MsDbContext>(option =>
                {

                    option.UseSqlServer(connectionString);
                });
            }

            void AssignBusValues(WebApplicationBuilder builder)
            {

                MessageBusConfig.Host = builder.Configuration["RabbitMQLogin:host"]!;
                MessageBusConfig.UserName = builder.Configuration["RabbitMQLogin:user"]!;
                MessageBusConfig.Password = builder.Configuration["RabbitMQLogin:password"]!;
                MessageBusConfig.QueueName = builder.Configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue")!;
            }

        }
        private static void AddMassTransit(WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, config) =>
                {
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }


}

