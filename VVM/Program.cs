using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VVM.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VVMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VVMContext") ?? throw new InvalidOperationException("Connection string 'VVMContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Admin/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Admin}/{action=Index}/{id?}");

app.Run();
