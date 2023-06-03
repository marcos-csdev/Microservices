using Microservices.AuthAPI;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//=================Adding DbContext========================
builder.Services.AddDbContext<MsDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
//=================Adding DbContext========================

//=================Configuring Token========================
//EntityFrameworkStores connects the identity framework with the db context
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("ApiSettings:JwtOptions"));
//=================Configuring Token========================


//=================Adding AutoMapper========================
var mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//=================Adding AutoMapper========================

//=================Adding Identity========================
//EntityFrameworkStores connects the identity framework with the db context
builder.Services.AddIdentity<MSUser, IdentityRole>()
    .AddEntityFrameworkStores<MsDbContext>()
    .AddDefaultTokenProviders();
//=================Adding Identity========================

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
