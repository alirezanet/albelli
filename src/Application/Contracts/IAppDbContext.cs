using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts;

public interface IAppDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderedProduct> OrderedProducts { get; }
    DbSet<ProductType> ProductTypes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}