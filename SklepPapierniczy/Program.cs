using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SklepPapierniczy.Data;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<SklepPapierniczyDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SklepPapierniczyDatabase")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
    options =>
    {
        options.IdleTimeout = TimeSpan.FromDays(7);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var env = app.Environment;

RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseRotativa();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "articleRoute",
    pattern: "{controller=ArticlesController}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "categoryRoute",
    pattern: "{controller=CategoriesController}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "CDRoute",
    pattern: "{controller=ClientDeliveriesController}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "OrderRoute",
    pattern: "{controller=OrderController}/{action=Index}/{id?}");

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
app.Run();
