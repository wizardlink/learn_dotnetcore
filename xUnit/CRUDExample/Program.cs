using CRUDExample.Middleware;
using CRUDExample.StartupExtensions;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // if (builder.Environment.IsDevelopment())
    //     app.UseDeveloperExceptionPage();
    // else
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseExceptionHandler("/Error");

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
