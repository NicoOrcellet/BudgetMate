using BudgetMate.Models;
using BudgetMate.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dependencias
builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<MoneyTransactionService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SavingLimitService>();
builder.Services.AddScoped<ConfigurationService>();

builder.Services.AddDbContext<MoneyManagerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MoneyManagerContext"));
});

var app = builder.Build();

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
