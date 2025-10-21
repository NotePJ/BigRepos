using HCBPCoreUI_Backend.Services;

// ...existing code...
using HCBPCoreUI_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ISelectService, SelectService>();
builder.Services.AddControllersWithViews();

// Register DbContext for SQL Server
builder.Services.AddDbContext<HRBudgetDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRBudgetDb")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(opts => {
  opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
