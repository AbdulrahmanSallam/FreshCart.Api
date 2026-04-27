using FreshCart.Domain.Common;

namespace FreshCart.Domain.Products;

public class ProductImage : BaseEntity
{
    public Guid ProductId { get; private set; }
    public string Url { get; private set; } = null!;
    public string AltText { get; private set; } = null!;
    public int SortOrder { get; private set; }

    public Product Product { get; private set; } = null!;

    private ProductImage() { }

    public ProductImage(Guid productId, string url, string altText, int sortOrder = 0)
    {
        ProductId = productId;
        Url = url;
        AltText = altText;
        SortOrder = sortOrder;
    }
}