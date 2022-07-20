using System.Net.Http;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.IntegrationTest;

public abstract class IntegrationTestBase
{
    protected readonly HttpClient TestClient;

    protected IntegrationTestBase()
    {
        var appFactory = new WebAPIApplication();
        TestClient = appFactory.CreateClient();
    }
}

internal class WebAPIApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public WebAPIApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            // Replace SQLite with in-memory database for tests
            services.AddScoped(sp => new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestsDB")
                .UseApplicationServiceProvider(sp)
                .Options);
        });

        return base.CreateHost(builder);
    }
}