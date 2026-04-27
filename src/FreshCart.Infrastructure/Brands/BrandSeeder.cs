using FreshCart.Domain.Products;


namespace FreshCart.Infrastructure.Products;

public static class BrandSeeder
{
    public static List<Brand> GetBrands()
    {
        return new List<Brand>
        {
            // Electronics Brands
            Brand.Create("Apple", "Premium electronics", website: "https://www.apple.com", sortOrder: 0),
            Brand.Create("Samsung", "Innovative technology", website: "https://www.samsung.com", sortOrder: 1),
            Brand.Create("Sony", "Electronics and entertainment", website: "https://www.sony.com", sortOrder: 2),
            Brand.Create("Dell", "Computers and accessories", website: "https://www.dell.com", sortOrder: 3),
            Brand.Create("Bose", "Premium audio equipment", website: "https://www.bose.com", sortOrder: 4),
            Brand.Create("Canon", "Cameras and imaging", website: "https://www.canon.com", sortOrder: 5),
            
            // Fashion Brands
            Brand.Create("Nike", "Athletic footwear and apparel", website: "https://www.nike.com", sortOrder: 6),
            Brand.Create("Adidas", "Sportswear and accessories", website: "https://www.adidas.com", sortOrder: 7),
            Brand.Create("Zara", "Fast fashion clothing", website: "https://www.zara.com", sortOrder: 8),
            Brand.Create("H&M", "Affordable fashion", website: "https://www.hm.com", sortOrder: 9),
            Brand.Create("Levi's", "Denim and casual wear", website: "https://www.levi.com", sortOrder: 10),
            Brand.Create("Gucci", "Luxury fashion", website: "https://www.gucci.com", sortOrder: 11),
            Brand.Create("Puma", "Sportswear and lifestyle", website: "https://www.puma.com", sortOrder: 12),
            Brand.Create("Under Armour", "Performance apparel", website: "https://www.underarmour.com", sortOrder: 13),
        };
    }
}