using Ardalis.SmartEnum;

namespace FreshCart.Domain.Products;

public class ProductStatus : SmartEnum<ProductStatus>
{
    public static readonly ProductStatus Draft = new(nameof(Draft), 0);
    public static readonly ProductStatus Active = new(nameof(Active), 1);
    public static readonly ProductStatus OutOfStock = new(nameof(OutOfStock), 2);
    public static readonly ProductStatus Discontinued = new(nameof(Discontinued), 3);

    public ProductStatus(string name, int value) : base(name, value)
    {
    }
}