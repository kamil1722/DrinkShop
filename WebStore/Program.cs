using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using Microsoft.AspNetCore.Identity;
using WebStore.AuthModule.Services;
using WebStore.AuthModule.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения из конфигурации (appsettings.json)
string connStr_ProductsContext = builder.Configuration.GetConnectionString("ProductsContext") ?? throw new InvalidOperationException("Connection string 'ProductsContext' not found.");
string connStr_MyIdentityDbContext = builder.Configuration.GetConnectionString("MyIdentityDbContext") ?? throw new InvalidOperationException("Connection string 'MyIdentityDbContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

//  Добавляем ASP.NET Identity:
builder.Services.AddDbContext<MyIdentityDbContext>(options => // Используйте MyIdentityDbContext
    options.UseSqlServer(connStr_MyIdentityDbContext, b => b.MigrationsAssembly("WebStore"))); // Указываем строку подключения и сборку миграций

builder.Services.AddDbContext<ProductsContext>(options =>
    options.UseSqlServer(connStr_ProductsContext)); // Используйте ту же строку подключения

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) // Настройте параметры Identity
    .AddEntityFrameworkStores<MyIdentityDbContext>(); // Указываем контекст для Identity

// Регистрация сервисов из AuthModule:
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); // Scoped подходит для веб-приложений
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();// Для доступа к HttpContext

//реализация RabbitMQ
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Admin/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();