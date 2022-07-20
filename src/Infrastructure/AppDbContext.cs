using System.Reflection;
using Application.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderedProduct> OrderedProducts => Set<OrderedProduct>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}