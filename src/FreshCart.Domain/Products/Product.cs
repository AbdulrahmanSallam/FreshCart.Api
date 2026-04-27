using ErrorOr;
using Throw;
using FreshCart.Domain.Common;
using FreshCart.Domain.Errors;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace FreshCart.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public decimal Price { get; private set; }
    public decimal? CompareAtPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public ProductStatus Status { get; private set; }
    public string ImageUrl { get; private set; } = null!;

    // Computed properties
    public decimal? DiscountPercentage => CalculateDiscountPercentage();
    public bool IsOnSale => CompareAtPrice.HasValue && CompareAtPrice > Price;
    public bool IsInStock => StockQuantity > 0 && Status == ProductStatus.Active;

    // Normalized properties
    public string NormalizedName { get; private set; } = null!;

    // Foreign keys
    public Guid CategoryId { get; private set; }
    public Guid? BrandId { get; private set; }

    // Navigation properties
    public Category Category { get; private set; } = null!;
    public Brand? Brand { get; private set; }
    public ICollection<ProductImage> Images { get; private set; } = new List<ProductImage>();

    private Product() { }

    private Product(
        string name,
        string description,
        decimal price,
        decimal? compareAtPrice,
        int stockQuantity,
        string imageUrl,
        Guid categoryId,
        Guid? brandId)
    {
        Name = name;
        Description = description;
        Slug = GenerateSlug(name);
        Price = price;
        CompareAtPrice = compareAtPrice;
        StockQuantity = stockQuantity;
        Status = ProductStatus.Draft;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        BrandId = brandId;

        NormalizedName = Normalize(name);
    }

    // Factory method
    public static Product Create(
        string name,
        string description,
        decimal price,
        decimal? compareAtPrice,
        int stockQuantity,
        string imageUrl,
        Guid categoryId,
        Guid? brandId = null)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(200);
        description.Throw().IfNullOrWhiteSpace(x => $"{nameof(Description)} is required");
        price.Throw().IfNegativeOrZero(x => $"{nameof(Price)} must be greater than zero");
        stockQuantity.Throw().IfNegative(x => $"{nameof(StockQuantity)} cannot be negative");
        imageUrl.Throw().IfNullOrWhiteSpace(x => $"{nameof(ImageUrl)} is required");

        if (compareAtPrice.HasValue)
        {
            compareAtPrice.Value
                .Throw()
                .IfNegativeOrZero(x => $"{nameof(CompareAtPrice)} must be greater than zero")
                .IfTrue(cp => cp <= price, $"{nameof(CompareAtPrice)} must be greater than {nameof(Price)}");
        }
        return new Product(name, description, price, compareAtPrice, stockQuantity, imageUrl, categoryId, brandId);
    }

    // Update methods
    public void UpdateDetails(string name, string description)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(200);
        description.Throw().IfNullOrWhiteSpace(x => $"{nameof(Description)} is required");

        Name = name;
        Description = description;
        Slug = GenerateSlug(name);
        NormalizedName = Normalize(name);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal price, decimal? compareAtPrice = null)
    {
        price.Throw("Price").IfNegativeOrZero();

        if (compareAtPrice.HasValue)
        {
            compareAtPrice.Value.Throw("CompareAtPrice")
                .IfNegativeOrZero()
                .IfGreaterThan(price);
        }

        Price = price;
        CompareAtPrice = compareAtPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateImage(string imageUrl)
    {
        imageUrl.Throw().IfNullOrWhiteSpace(x => $"{nameof(ImageUrl)} is required");

        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    // Business rules - return ErrorOr
    public ErrorOr<Success> AssignCategory(Guid categoryId)
    {
        if (CategoryId == categoryId)
            return ProductErrors.AlreadyAssignedToCategory;

        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> AssignBrand(Guid brandId)
    {
        if (BrandId == brandId)
            return ProductErrors.AlreadyAssignedToBrand;

        BrandId = brandId;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public void RemoveBrand()
    {
        BrandId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public ErrorOr<Success> AdjustStock(int quantity)
    {
        var newQuantity = StockQuantity + quantity;

        if (newQuantity < 0)
            return ProductErrors.InsufficientStock;

        StockQuantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;

        if (StockQuantity == 0 && Status == ProductStatus.Active)
            Status = ProductStatus.OutOfStock;

        return Result.Success;
    }

    public ErrorOr<Success> AddImage(string url, string altText, int maxImages = 10)
    {
        url.Throw().IfNullOrWhiteSpace(x => $"{nameof(ImageUrl)} is required");

        if (Images.Count >= maxImages)
            return ProductErrors.MaxImagesReached(maxImages);

        Images.Add(new ProductImage(Id, url, altText, Images.Count));
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> RemoveImage(Guid imageId)
    {
        var image = Images.FirstOrDefault(x => x.Id == imageId);

        if (image is null)
            return ProductErrors.ImageNotFound;

        Images.Remove(image);
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    // Status management
    public ErrorOr<Success> Publish()
    {
        if (Status == ProductStatus.Discontinued)
            return ProductErrors.CannotPublishDiscontinuedProduct;

        if (StockQuantity <= 0)
            return ProductErrors.CannotPublishOutOfStockProduct;

        if (string.IsNullOrWhiteSpace(ImageUrl))
            return ProductErrors.ImageRequiredForPublishing;

        Status = ProductStatus.Active;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> Draft()
    {
        if (Status == ProductStatus.Discontinued)
            return ProductErrors.CannotDraftDiscontinuedProduct;

        Status = ProductStatus.Draft;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public void SetOutOfStock()
    {
        Status = ProductStatus.OutOfStock;
        UpdatedAt = DateTime.UtcNow;
    }

    public ErrorOr<Success> Discontinue()
    {
        if (Status == ProductStatus.Discontinued)
            return ProductErrors.AlreadyDiscontinued;

        Status = ProductStatus.Discontinued;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    // Private helpers
    private decimal? CalculateDiscountPercentage()
    {
        if (!CompareAtPrice.HasValue || CompareAtPrice.Value <= 0 || CompareAtPrice <= Price)
            return null;

        return Math.Round((CompareAtPrice.Value - Price) / CompareAtPrice.Value * 100, 2);
    }

    private static string GenerateSlug(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        // Remove diacritics (accents)
        var normalizedString = name.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        // Convert to lowercase
        slug = slug.ToLowerInvariant();

        // Replace & with "and"
        slug = slug.Replace("&", "and");

        // Replace special characters with spaces
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", " ");

        // Replace multiple spaces/hyphens with single hyphen
        slug = Regex.Replace(slug, @"[\s-]+", "-");

        // Trim hyphens
        slug = slug.Trim('-');

        return slug;
    }
    private static string Normalize(string value)
        => value.ToUpperInvariant().Trim();
}