namespace Domain.Products.Error;

public static class ProductErrors
{
    public static Shared.Error ProductNotFound => Shared.Error.NotFound("Product.Not.Found", "The product was not found");
}