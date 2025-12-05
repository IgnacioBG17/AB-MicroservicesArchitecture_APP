using Ecommerce.Web.Service;
using Ecommerce.Web.Service.IService;
using Ecommerce.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/* Registramos al contenedor de dependencias el servicio IHttpContextAccessor 
 * y registramos el factory HttpClient y un HttpClient tipado */
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

/* Cargamos desde la configuracion la URL del servicio y la asignamos a la variable CouponAPIBase */
SD.CouponAPIBase = builder.Configuration["ServicesUrls:CouponAPI"];
SD.AuthAPIBase = builder.Configuration["ServicesUrls:AuthAPI"];
SD.ProductAPIBase = builder.Configuration["ServicesUrls:ProductAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServicesUrls:ShoppingCartAPI"];
SD.OrderAPIBase = builder.Configuration["ServicesUrls:OrderAPI"];

// Registro de servicio en el contenedor de dependencias
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Configuración de autenticación basada en cookies:
// - Despues de 10 minutos de inactividad la sesion expira
// - Define la ruta de redirección para Login y para acceso denegado
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";

    /* Para redigir al usuario al login sin importar si la ruta esta protegida o no */
    //options.Events.OnRedirectToLogin = context =>
    //{
    //    context.Response.Redirect("/Auth/Login");
    //    return Task.CompletedTask;
    //};
});

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
app.UseAuthentication(); /* permitimos que use Autenticacion */
app.UseAuthorization();

/* Para redigir al usuario al login sin importar si la ruta esta protegida o no */
//app.Use(async (context, next) =>
//{
//    if (!context.User.Identity.IsAuthenticated &&
//        !context.Request.Path.StartsWithSegments("/Auth"))
//    {
//        context.Response.Redirect("/Auth/Login");
//        return;
//    }

//    await next();
//});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
