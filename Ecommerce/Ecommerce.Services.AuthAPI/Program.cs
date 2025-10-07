using Ecommerce.Services.AuthAPI.Data;
using Ecommerce.Services.AuthAPI.Models;
using Ecommerce.Services.AuthAPI.Service;
using Ecommerce.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


/* Configuraci�n cadena de conexion */
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/* Configuraci�n JWT */
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

/* Configuraci�n de autenticaci�n y autorizaci�n con Identity en Ecommerce.service.AuthAPI */
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// Add services to the container.
builder.Services.AddControllers();

/* Configuraci�n Swagger */
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /* Configuraci�n Swagger */
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication(); /* usamos autenticaci�n */
app.MapControllers();
ApplyMigration();
app.Run();


/* Aplicar la migraci�n al arrancar el servicio */
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}