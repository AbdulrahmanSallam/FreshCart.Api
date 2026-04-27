using Microsoft.EntityFrameworkCore;
using FreshCart.Domain.Products;

namespace FreshCart.Infrastructure.Common.Persistence;

public class FreshCartDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public FreshCartDbContext(DbContextOptions<FreshCartDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreshCartDbContext).Assembly);
    }
}