using FreshCart.Domain.Products;

namespace FreshCart.Infrastructure.Products;

public static class ProductSeeder
{
    public static List<Product> GetProducts(
        List<Category> categories,
        List<Brand> brands)
    {
        // Get category IDs by Name (never breaks)
        var smartphones = categories.First(c => c.Name == "Smartphones").Id;
        var laptops = categories.First(c => c.Name == "Laptops").Id;
        var tablets = categories.First(c => c.Name == "Tablets").Id;
        var headphones = categories.First(c => c.Name == "Headphones & Audio").Id;
        var cameras = categories.First(c => c.Name == "Cameras").Id;
        var elecAccessories = categories.First(c => c.Name == "Electronics Accessories").Id;

        var menShirts = categories.First(c => c.Name == "Men's Shirts").Id;
        var menPants = categories.First(c => c.Name == "Men's Pants").Id;
        var menShoes = categories.First(c => c.Name == "Men's Shoes").Id;
        var menAccessories = categories.First(c => c.Name == "Men's Accessories").Id;

        var womenDresses = categories.First(c => c.Name == "Women's Dresses").Id;
        var womenTops = categories.First(c => c.Name == "Women's Tops").Id;
        var womenShoes = categories.First(c => c.Name == "Women's Shoes").Id;
        var womenBags = categories.First(c => c.Name == "Women's Bags").Id;

        // Get brand IDs by Name (never breaks)
        var apple = brands.First(b => b.Name == "Apple").Id;
        var samsung = brands.First(b => b.Name == "Samsung").Id;
        var sony = brands.First(b => b.Name == "Sony").Id;
        var dell = brands.First(b => b.Name == "Dell").Id;
        var bose = brands.First(b => b.Name == "Bose").Id;
        var canon = brands.First(b => b.Name == "Canon").Id;
        var nike = brands.First(b => b.Name == "Nike").Id;
        var adidas = brands.First(b => b.Name == "Adidas").Id;
        var zara = brands.First(b => b.Name == "Zara").Id;
        var hm = brands.First(b => b.Name == "H&M").Id;
        var levis = brands.First(b => b.Name == "Levi's").Id;
        var gucci = brands.First(b => b.Name == "Gucci").Id;

        return new List<Product>
        {
            // Smartphones
            CreateProduct("iPhone 15 Pro", "Apple's latest flagship smartphone with A17 Pro chip", 999.99m, 1099.99m, 50, smartphones, apple),
            CreateProduct("Samsung Galaxy S24 Ultra", "Samsung's premium smartphone with AI features", 1199.99m, 1299.99m, 35, smartphones, samsung),
            
            // Laptops
            CreateProduct("MacBook Pro 16", "Powerful laptop with M3 Pro chip", 2499.99m, null, 20, laptops, apple),
            CreateProduct("Dell XPS 15", "Premium Windows laptop with OLED display", 1899.99m, 2099.99m, 15, laptops, dell),
            
            // Tablets
            CreateProduct("iPad Air", "Lightweight tablet with M2 chip", 599.99m, null, 40, tablets, apple),
            CreateProduct("Samsung Galaxy Tab S9", "Android tablet with S Pen", 799.99m, 899.99m, 25, tablets, samsung),
            
            // Headphones
            CreateProduct("AirPods Pro 2", "Wireless earbuds with active noise cancellation", 249.99m, null, 100, headphones, apple),
            CreateProduct("Bose QuietComfort Ultra", "Premium noise-cancelling headphones", 429.99m, 449.99m, 30, headphones, bose),
            
            // Cameras
            CreateProduct("Canon EOS R6 Mark II", "Full-frame mirrorless camera", 2499.99m, null, 10, cameras, canon),
            CreateProduct("Sony Alpha 7 IV", "Full-frame hybrid camera", 2399.99m, 2599.99m, 8, cameras, sony),
            
            // Electronics Accessories
            CreateProduct("Apple 20W USB-C Charger", "Fast charging adapter", 19.99m, null, 200, elecAccessories, apple),
            CreateProduct("Samsung 45W Charger", "Super fast charging adapter", 49.99m, null, 150, elecAccessories, samsung),
            
            // Men's Shirts
            CreateProduct("Classic Oxford Shirt", "Timeless button-down shirt", 59.99m, 79.99m, 80, menShirts, zara),
            CreateProduct("Linen Casual Shirt", "Lightweight summer shirt", 49.99m, null, 60, menShirts, hm),
            
            // Men's Pants
            CreateProduct("Slim Fit Chinos", "Modern slim fit cotton chinos", 69.99m, 89.99m, 70, menPants, levis),
            CreateProduct("Classic 501 Jeans", "Original fit denim jeans", 89.99m, null, 90, menPants, levis),
            
            // Men's Shoes
            CreateProduct("Nike Air Max 270", "Comfortable lifestyle sneakers", 149.99m, 169.99m, 45, menShoes, nike),
            CreateProduct("Adidas Ultraboost", "Running shoes with Boost technology", 179.99m, null, 40, menShoes, adidas),
            
            // Men's Accessories
            CreateProduct("Leather Belt", "Genuine leather dress belt", 39.99m, null, 100, menAccessories, levis),
            CreateProduct("Classic Watch", "Minimalist analog watch", 199.99m, 249.99m, 30, menAccessories, gucci),
            
            // Women's Dresses
            CreateProduct("Floral Summer Dress", "Light and elegant floral dress", 79.99m, 99.99m, 50, womenDresses, zara),
            CreateProduct("Evening Gown", "Elegant floor-length gown", 299.99m, null, 15, womenDresses, gucci),
            
            // Women's Tops
            CreateProduct("Silk Blouse", "Classic silk blouse for office wear", 89.99m, 119.99m, 55, womenTops, zara),
            CreateProduct("Cotton T-Shirt", "Everyday essential crew neck tee", 19.99m, null, 120, womenTops, hm),
            
            // Women's Shoes
            CreateProduct("Classic Pumps", "Elegant high heel pumps", 129.99m, 159.99m, 35, womenShoes, gucci),
            CreateProduct("Running Shoes", "Lightweight running sneakers", 139.99m, null, 50, womenShoes, nike),
            
            // Women's Bags
            CreateProduct("Leather Tote Bag", "Spacious everyday tote", 199.99m, 249.99m, 25, womenBags, gucci),
            CreateProduct("Crossbody Bag", "Compact and stylish crossbody", 79.99m, null, 40, womenBags, zara),
        };
    }

    private static Product CreateProduct(
        string name,
        string description,
        decimal price,
        decimal? compareAtPrice,
        int stockQuantity,
        Guid categoryId,
        Guid brandId,
        string imageUrl = "https://placehold.co/600x400")
    {
        var product = Product.Create(name, description, price, compareAtPrice, stockQuantity, imageUrl, categoryId, brandId);
        product.Publish();
        return product;
    }
}