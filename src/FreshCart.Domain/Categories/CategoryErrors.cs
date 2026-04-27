using ErrorOr;

namespace FreshCart.Domain.Errors;

public static class CategoryErrors
{
    public static Error AlreadyActive => Error.Conflict(
        "Category.AlreadyActive",
        "Category is already active");

    public static Error AlreadyInactive => Error.Conflict(
        "Category.AlreadyInactive",
        "Category is already inactive");

    public static Error CannotDeactivateWithActiveProducts => Error.Conflict(
        "Category.CannotDeactivateWithActiveProducts",
        "Cannot deactivate a category that has active products");

    public static Error AlreadyInThisCategory => Error.Conflict(
        "Category.AlreadyInThisCategory",
        "Category is already assigned to this parent");

    public static Error CannotBeOwnParent => Error.Validation(
        "Category.CannotBeOwnParent",
        "A category cannot be its own parent");
}