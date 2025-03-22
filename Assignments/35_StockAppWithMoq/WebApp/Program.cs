using Microsoft.EntityFrameworkCore;
using Services;
using Services.Contracts;
using Services.Repositories;
using WebApp.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.AddScoped<IStocksRepository, StocksRepository>();

builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

builder.Services.AddDbContext<DatabaseContext>(context =>
{
    context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }
