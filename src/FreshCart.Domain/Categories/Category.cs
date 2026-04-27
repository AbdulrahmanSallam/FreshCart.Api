using ErrorOr;
using Throw;
using FreshCart.Domain.Common;
using FreshCart.Domain.Errors;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace FreshCart.Domain.Products;

public class Category : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    // Computed properties
    public bool IsRoot => ParentCategoryId == null;

    // Normalized properties
    public string NormalizedName { get; private set; } = null!;

    // Self-referencing hierarchy
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> SubCategories { get; private set; } = new List<Category>();

    // Navigation properties
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category() { }

    private Category(
        string name,
        string? description,
        string? imageUrl,
        Guid? parentCategoryId,
        int sortOrder)
    {
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        ImageUrl = imageUrl;
        ParentCategoryId = parentCategoryId;
        SortOrder = sortOrder;
        IsActive = true;
        NormalizedName = Normalize(name);
    }

    // Factory method
    public static Category Create(
        string name,
        string? description = null,
        string? imageUrl = null,
        Guid? parentCategoryId = null,
        int sortOrder = 0)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(100);

        description?.Throw()
            .IfLongerThan(x => $"{nameof(Description)} must be less than 500 characters", 500);

        imageUrl?.Throw()
            .IfLongerThan(x => $"{nameof(ImageUrl)} must be less than 500 characters", 500);

        sortOrder.Throw()
            .IfNegative(x => $"{nameof(SortOrder)} cannot be negative");

        return new Category(name, description, imageUrl, parentCategoryId, sortOrder);
    }

    // Update methods
    public void Update(
        string name,
        string? description = null,
        string? imageUrl = null,
        int? sortOrder = null)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(100);

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        ImageUrl = imageUrl;
        SortOrder = sortOrder ?? SortOrder;
        NormalizedName = Normalize(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Business rules
    public ErrorOr<Success> MoveToParent(Guid? parentCategoryId)
    {
        if (ParentCategoryId == parentCategoryId)
            return CategoryErrors.AlreadyInThisCategory;

        if (parentCategoryId == Id)
            return CategoryErrors.CannotBeOwnParent;

        ParentCategoryId = parentCategoryId;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> Activate()
    {
        if (IsActive)
            return CategoryErrors.AlreadyActive;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> Deactivate()
    {
        if (!IsActive)
            return CategoryErrors.AlreadyInactive;

        if (Products.Any(p => p.Status == ProductStatus.Active))
            return CategoryErrors.CannotDeactivateWithActiveProducts;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    // helpers
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


    private static string Normalize(string? value)
        => value?.ToUpperInvariant().Trim() ?? string.Empty;
}