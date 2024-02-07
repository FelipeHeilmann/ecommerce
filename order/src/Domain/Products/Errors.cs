using Domain.Shared;

namespace Domain.Products;

public static class ProductErrors
{
    public static Error SkuNull => Error.Validation("Sku.Null.Empty", "The sku value should not be empty or null");
    public static Error SkuLength => Error.Validation("Sku.Length", "The sku length should be less than 15");
    public static Error CategoryNotFound => Error.NotFound("Category.Not.Found", "The category was not found");
    public static Error ProductNotFound => Error.NotFound("Product.Not.Found", "The product was not found");
}