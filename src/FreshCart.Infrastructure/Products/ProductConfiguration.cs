

using FreshCart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshCart.Infrastructure.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        // Table
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
        .ValueGeneratedNever();

        // Properties
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.CompareAtPrice)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => ProductStatus.FromValue(v));

        builder.Property(p => p.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        // Computed properties - ignored for EF
        builder.Ignore(p => p.DiscountPercentage);
        builder.Ignore(p => p.IsOnSale);
        builder.Ignore(p => p.IsInStock);

        // Normalized properties
        builder.Property(p => p.NormalizedName)
            .IsRequired()
            .HasMaxLength(200);

        // Foreign Keys
        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.Property(p => p.BrandId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.NormalizedName);
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.BrandId);
        builder.HasIndex(p => p.Price);
        builder.HasIndex(p => p.Status);

        // Relationships
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}