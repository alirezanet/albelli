using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Default")));
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<AppDbContextInitializer>();

        return services;
    }

    public static async Task InitializeDb(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();
        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}