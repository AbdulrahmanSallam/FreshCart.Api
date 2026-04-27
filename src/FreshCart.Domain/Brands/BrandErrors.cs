using ErrorOr;

namespace FreshCart.Domain.Errors;

public static class BrandErrors
{
    public static Error AlreadyActive => Error.Conflict(
        "Brand.AlreadyActive",
        "Brand is already active");

    public static Error AlreadyInactive => Error.Conflict(
        "Brand.AlreadyInactive",
        "Brand is already inactive");

    public static Error CannotDeactivateWithActiveProducts => Error.Conflict(
        "Brand.CannotDeactivateWithActiveProducts",
        "Cannot deactivate a brand that has active products");
}