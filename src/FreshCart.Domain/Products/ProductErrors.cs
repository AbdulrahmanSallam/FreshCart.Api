using ErrorOr;

namespace FreshCart.Domain.Errors;

public static class ProductErrors
{
    public static Error CannotPublishDiscontinuedProduct => Error.Conflict(
        "Product.CannotPublishDiscontinued",
        "Cannot publish a discontinued product");

    public static Error CannotPublishOutOfStockProduct => Error.Conflict(
        "Product.CannotPublishOutOfStock",
        "Cannot publish a product with zero stock");

    public static Error ImageRequiredForPublishing => Error.Validation(
        "Product.ImageRequired",
        "Product must have an image before publishing");

    public static Error CannotDraftDiscontinuedProduct => Error.Conflict(
        "Product.CannotDraftDiscontinued",
        "Cannot move a discontinued product back to draft");

    public static Error InsufficientStock => Error.Conflict(
        "Product.InsufficientStock",
        "Not enough stock available");

    public static Error AlreadyDiscontinued => Error.Conflict(
        "Product.AlreadyDiscontinued",
        "Product is already discontinued");

    public static Error AlreadyAssignedToCategory => Error.Conflict(
        "Product.AlreadyAssignedToCategory",
        "Product is already assigned to this category");

    public static Error AlreadyAssignedToBrand => Error.Conflict(
        "Product.AlreadyAssignedToBrand",
        "Product is already assigned to this brand");

    public static Error MaxImagesReached(int max) => Error.Conflict(
        "Product.MaxImagesReached",
        $"Product cannot have more than {max} images");

    public static Error ImageNotFound => Error.NotFound(
        "Product.ImageNotFound",
        "Image not found on this product");
}