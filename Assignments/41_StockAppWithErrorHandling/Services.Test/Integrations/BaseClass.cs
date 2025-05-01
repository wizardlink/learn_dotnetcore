using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Services.Test.Integrations;

public class IntegrationClientFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<DatabaseContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<DatabaseContext>>();

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("TestDB");
            });
        });
    }
}

public abstract class BaseClass(IntegrationClientFactory factory) : IClassFixture<IntegrationClientFactory>
{
    protected readonly Fixture _fixture = new();
    protected readonly HttpClient _client = factory.CreateClient();
}
