using FreshCart.Infrastructure.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FreshCart.Infrastructure.Common.Persistence;

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<FreshCartDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FreshCartDbContext>>();

        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Seed data if empty
        if (!await context.Categories.AnyAsync())
        {
            logger.LogInformation("Seeding categories...");
            var categories = CategorySeeder.GetCategories();
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        if (!await context.Brands.AnyAsync())
        {
            logger.LogInformation("Seeding brands...");
            var brands = BrandSeeder.GetBrands();
            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            logger.LogInformation("Seeding products...");
            var categories = await context.Categories.ToListAsync();
            var brands = await context.Brands.ToListAsync();
            var products = ProductSeeder.GetProducts(categories, brands);
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("Database seeding completed successfully.");
    }
}