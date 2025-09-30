using AutoMapper;
using Ecommerce.Services.CouponAPI;
using Ecommerce.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;

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

app.UseHttpsRedirection();

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