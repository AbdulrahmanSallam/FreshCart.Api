using FreshCart.Domain.Products;

namespace FreshCart.Infrastructure.Products;

public static class CategorySeeder
{
    public static List<Category> GetCategories()
    {
        // --- Electronics ---
        var electronics = Category.Create("Electronics", "Electronic devices and gadgets", sortOrder: 0);
        var smartphones = Category.Create("Smartphones", "Mobile phones", parentCategoryId: electronics.Id, sortOrder: 0);
        var laptops = Category.Create("Laptops", "Notebooks and laptops", parentCategoryId: electronics.Id, sortOrder: 1);
        var tablets = Category.Create("Tablets", "Tablets and iPads", parentCategoryId: electronics.Id, sortOrder: 2);
        var headphones = Category.Create("Headphones & Audio", "Headphones, earbuds, speakers", parentCategoryId: electronics.Id, sortOrder: 3);
        var cameras = Category.Create("Cameras", "Digital cameras and accessories", parentCategoryId: electronics.Id, sortOrder: 4);
        var elecAccessories = Category.Create("Electronics Accessories", "Chargers, cases, cables", parentCategoryId: electronics.Id, sortOrder: 5);

        // --- Fashion ---
        var fashion = Category.Create("Fashion", "Clothing and accessories", sortOrder: 1);

        // Men
        var men = Category.Create("Men", "Men's fashion", parentCategoryId: fashion.Id, sortOrder: 0);
        var menShirts = Category.Create("Men's Shirts", "Men's shirts", parentCategoryId: men.Id, sortOrder: 0);
        var menPants = Category.Create("Men's Pants", "Men's pants and jeans", parentCategoryId: men.Id, sortOrder: 1);
        var menShoes = Category.Create("Men's Shoes", "Men's shoes", parentCategoryId: men.Id, sortOrder: 2);
        var menAccessories = Category.Create("Men's Accessories", "Watches, belts, wallets", parentCategoryId: men.Id, sortOrder: 3);

        // Women
        var women = Category.Create("Women", "Women's fashion", parentCategoryId: fashion.Id, sortOrder: 1);
        var womenDresses = Category.Create("Women's Dresses", "Women's dresses", parentCategoryId: women.Id, sortOrder: 0);
        var womenTops = Category.Create("Women's Tops", "Blouses, shirts, t-shirts", parentCategoryId: women.Id, sortOrder: 1);
        var womenShoes = Category.Create("Women's Shoes", "Women's shoes and heels", parentCategoryId: women.Id, sortOrder: 2);
        var womenBags = Category.Create("Women's Bags", "Handbags, purses, backpacks", parentCategoryId: women.Id, sortOrder: 3);

        // Kids
        var kids = Category.Create("Kids", "Kids clothing and accessories", parentCategoryId: fashion.Id, sortOrder: 2);

        return new List<Category>
    {
        electronics, smartphones, laptops, tablets, headphones, cameras, elecAccessories,
        fashion,
        men, menShirts, menPants, menShoes, menAccessories,
        women, womenDresses, womenTops, womenShoes, womenBags,
        kids
    };
    }
}