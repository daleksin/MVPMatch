using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVPMatch.Core.Models;

namespace MVPMatch.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(e => e.ProductId)
                .ValueGeneratedOnAdd();
            builder
                .Property(e => e.CostEur)
                .IsRequired();
            builder
                .Property(e => e.SellerId)
                .IsRequired();
            builder
                .Property(e => e.ProductName)
                .IsRequired();
        }
    }
}
