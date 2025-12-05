using AutoMapper;
using Ecommerce.MessageBus;
using Ecommerce.Services.OrderAPI;
using Ecommerce.Services.OrderAPI.Data;
using Ecommerce.Services.OrderAPI.Extensions;
using Ecommerce.Services.OrderAPI.Service;
using Ecommerce.Services.OrderAPI.Service.IService;
using Ecommerce.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

/* Configuración cadena de conexion */
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/* Configuración de AutoMapper */
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registro de servicio en el contenedor de dependencias
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductService, Ecommerce.Services.OrderAPI.Service.ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<BackEndApiAuthenticationHttpClientHandler>();

// HttpClient para consumir microservicios 
builder.Services.AddHttpClient("Product", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]))
                .AddHttpMessageHandler<BackEndApiAuthenticationHttpClientHandler>();

builder.Services.AddControllers();

/* Configuración Swagger */
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the bearer Authorization string as following: `Bearer Generated-JWT-Token`",
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
            }, new string[]{}
        }
    });
});


/* Configuración para que el servicio pueda reconocer el token generado por al Auth.Api */
builder.AddAppAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /* Configuración Swagger */
    app.UseSwagger();
    app.UseSwaggerUI();
}

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseHttpsRedirection();
app.UseAuthentication(); /* Para que el frontEnd pueda consumir este backend */
app.UseAuthorization();

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