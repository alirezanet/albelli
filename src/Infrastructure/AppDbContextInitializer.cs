using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class AppDbContextInitializer
{
    private readonly AppDbContext _context;
    private readonly ILogger<AppDbContextInitializer> _logger;

    public AppDbContextInitializer(ILogger<AppDbContextInitializer> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            // throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.ProductTypes.Any())
        {
            await _context.ProductTypes.AddRangeAsync(
                new ProductType
                {
                    Id = 1,
                    Name = "PhotoBook",
                    StackSize = 1,
                    RequiredBinWidth = 19
                },
                new ProductType
                {
                    Id = 2,
                    Name = "Calendar",
                    StackSize = 1,
                    RequiredBinWidth = 10
                },
                new ProductType
                {
                    Id = 3,
                    Name = "Canvas",
                    StackSize = 1,
                    RequiredBinWidth = 16
                },
                new ProductType
                {
                    Id = 4,
                    Name = "Cards",
                    StackSize = 1,
                    RequiredBinWidth = 4.7
                },
                new ProductType
                {
                    Id = 5,
                    Name = "Mug",
                    StackSize = 4,
                    RequiredBinWidth = 94
                });

            await _context.SaveChangesAsync();
        }
    }
}