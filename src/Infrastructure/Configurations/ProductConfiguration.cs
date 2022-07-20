using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.Property(q => q.Name).IsRequired().HasMaxLength(200);
        builder.Property(q => q.StackSize).HasDefaultValue(1);
        builder.HasMany(q => q.OrderedProducts).WithOne(q => q.ProductType).HasForeignKey(q => q.ProductTypeId);
    }
}