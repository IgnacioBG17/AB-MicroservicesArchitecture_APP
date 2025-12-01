using Ecommerce.MessageBus;
using Ecommerce.Services.AuthAPI.Data;
using Ecommerce.Services.AuthAPI.Models;
using Ecommerce.Services.AuthAPI.Service;
using Ecommerce.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Configuración cadena de conexion */
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/* Configuración JWT */
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

/* Configuración de autenticación y autorización con Identity en Ecommerce.service.AuthAPI */
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Registro de servicio en el contenedor de dependencias
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

// Add services to the container.
builder.Services.AddControllers();

/* Configuración Swagger */
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /* Configuración Swagger */
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication(); /* usamos autenticación */
app.MapControllers();
ApplyMigration();
app.Run();


/* Aplicar la migración al arrancar el servicio */
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