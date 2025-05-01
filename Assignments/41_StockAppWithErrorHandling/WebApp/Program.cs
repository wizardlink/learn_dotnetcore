using Microsoft.EntityFrameworkCore;
using Serilog;
using Services;
using Services.Contracts;
using Services.Repositories;
using WebApp.Configuration;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("Application is starting...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog(
        (services, lc) =>
            lc
                .MinimumLevel.Information()
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
    );

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

    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields =
            Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
            | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler("/Home/Error");

    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.Information("Application has closed.");
    Log.CloseAndFlush();
}

public partial class Program { }
