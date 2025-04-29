using CRUDExample.Filters.ActionFilters;
using CRUDExample.Middleware;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;

namespace CRUDExample.StartupExtensions;

public static class ConfigureServicesExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
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
            options.LoggingFields =
                HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
        });

        builder.Services.AddTransient<PersonsListActionFilter>();
    }
}
