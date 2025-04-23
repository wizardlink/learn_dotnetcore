using CRUDExample.Filters.ActionFilters;
using CRUDExample.StartupExtensions;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    builder.Services.AddSerilog(
        (services, lc) =>
            lc
                .MinimumLevel.Information()
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "CRUD Demo App")
                .WriteTo.Console() //.WriteTo.SQLite(Environment.CurrentDirectory + @"/logs/log.db")
    );

    builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add<ResponseHeaderActionFilter.Global>();
    });

    builder.Services.AddScoped<ICountriesService, CountriesService>();
    builder.Services.AddScoped<IPersonService, PersonsService>();

    builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
    builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultDatabase"));
    });

    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
    });

    builder.Services.AddTransient<PersonsListActionFilter>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // app.Logger.LogDebug("debug-message");
    // app.Logger.LogInformation("information-message");
    // app.Logger.LogWarning("warning-message");
    // app.Logger.LogError("error-message");
    // app.Logger.LogCritical("critical-message");

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
    Log.CloseAndFlush();
}

public partial class Program { }
