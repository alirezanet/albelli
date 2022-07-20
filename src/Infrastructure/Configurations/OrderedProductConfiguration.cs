using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrderedProductConfiguration : IEntityTypeConfiguration<OrderedProduct>
{
    public void Configure(EntityTypeBuilder<OrderedProduct> builder)
    {
        builder.HasOne(q => q.Order).WithMany(q => q.Products).HasForeignKey(q => q.OrderId);
        builder.HasOne(q => q.ProductType).WithMany(q => q.OrderedProducts).HasForeignKey(q => q.ProductTypeId);
    }
}