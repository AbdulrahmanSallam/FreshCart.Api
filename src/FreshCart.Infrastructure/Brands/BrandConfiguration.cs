

using FreshCart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshCart.Infrastructure.Brands;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        // Table
        builder.ToTable("Brands");

        // Primary Key
        builder.HasKey(b => b.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        // Properties
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Slug)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.Property(b => b.LogoUrl)
            .HasMaxLength(500);

        builder.Property(b => b.Website)
            .HasMaxLength(300);

        builder.Property(b => b.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Normalized properties
        builder.Property(b => b.NormalizedName)
            .IsRequired()
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(b => b.Slug).IsUnique();
        builder.HasIndex(b => b.NormalizedName);
        builder.HasIndex(b => b.IsActive);
        builder.HasIndex(b => b.SortOrder);
    }
}