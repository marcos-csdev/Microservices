using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Microservices.EmailAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AutenticateJwtTokenExtension(this WebApplicationBuilder builder)
        {
            var configSection = builder.Configuration.GetSection("ApiSettings:JwtOptions");

            var secret = configSection.GetValue<string>("Secret");
            var issuer = configSection.GetValue<string>("Issuer");
            var audience = configSection.GetValue<string>("Audience");

            var securityKey = Encoding.UTF8.GetBytes(secret!);

            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(token =>
            {
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience
                };
            });

            builder.Services.AddAuthorization();

            return builder;
        }

        //Adding authentication to swagger doc
        public static WebApplicationBuilder AddSwaggerGenConfigExtension(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the bearer token as: `Bearer Generated-JWT-Token-at-login-from-AuthAPI`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                                new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                        Array.Empty<string>()
                        }
                    });
            });

            return builder;
        }
    }
}
