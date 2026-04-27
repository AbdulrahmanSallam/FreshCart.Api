using FreshCart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshCart.Infrastructure.Images;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        // Table
        builder.ToTable("ProductImages");

        // Primary Key
        builder.HasKey(i => i.Id);

        // Properties
        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.AltText)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        // Foreign Keys
        builder.Property(i => i.ProductId)
            .IsRequired();

        // Indexes
        builder.HasIndex(i => i.ProductId);
        builder.HasIndex(i => i.SortOrder);

        // Relationship
        builder.HasOne(i => i.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}