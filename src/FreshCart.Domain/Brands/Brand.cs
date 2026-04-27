using ErrorOr;
using Throw;
using FreshCart.Domain.Common;
using FreshCart.Domain.Errors;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace FreshCart.Domain.Products;

public class Brand : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? Website { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }

    // Normalized properties
    public string NormalizedName { get; private set; } = null!;

    // Navigation properties
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Brand() { }

    private Brand(
        string name,
        string? description,
        string? logoUrl,
        string? website,
        int sortOrder)
    {
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        LogoUrl = logoUrl;
        Website = website;
        SortOrder = sortOrder;
        IsActive = true;

        NormalizedName = Normalize(name);
    }

    // Factory method
    public static Brand Create(
        string name,
        string? description = null,
        string? logoUrl = null,
        string? website = null,
        int sortOrder = 0)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(100);

        if (!string.IsNullOrWhiteSpace(website))
        {
            website.Throw().IfTrue(
                w => !Uri.TryCreate(w, UriKind.Absolute, out var uri)
                     || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps),
                $"{nameof(Website)} must be a valid URL");
        }

        return new Brand(name, description, logoUrl, website, sortOrder);
    }

    // Update methods
    public void Update(
        string name,
        string? description = null,
        string? logoUrl = null,
        string? website = null)
    {
        name.Throw().IfNullOrWhiteSpace(x => $"{nameof(Name)} is required").IfLongerThan(100);

        if (!string.IsNullOrWhiteSpace(website))
        {
            website.Throw().IfTrue(
                w => !Uri.TryCreate(w, UriKind.Absolute, out var uri)
                     || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps),
                $"{nameof(Website)} must be a valid URL");
        }

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        LogoUrl = logoUrl;
        Website = website;
        NormalizedName = Normalize(name);
        UpdatedAt = DateTime.UtcNow;
    }

    // Business rules
    public ErrorOr<Success> Activate()
    {
        if (IsActive)
            return BrandErrors.AlreadyActive;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> Deactivate()
    {
        if (!IsActive)
            return BrandErrors.AlreadyInactive;

        if (Products.Any(p => p.Status == ProductStatus.Active))
            return BrandErrors.CannotDeactivateWithActiveProducts;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success;
    }

    // Private helpers
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