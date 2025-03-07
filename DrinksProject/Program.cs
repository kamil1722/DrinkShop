﻿using Microsoft.EntityFrameworkCore;
using DrinksProject.Data;
using Microsoft.AspNetCore.Identity;
using Drinks.AuthModule.Services;
using Drinks.AuthModule.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения из конфигурации (appsettings.json)
string connectionString = builder.Configuration.GetConnectionString("MyContext") ?? throw new InvalidOperationException("Connection string 'MyContext' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();

//  Добавляем ASP.NET Identity:
builder.Services.AddDbContext<MyIdentityDbContext>(options => // Используйте MyIdentityDbContext
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DrinksProject"))); // Указываем строку подключения и сборку миграций

builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlServer(connectionString)); // Используйте ту же строку подключения

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