namespace Application.Products.Model;

public record CreateProductModel(string Name, string Description, string Currency, double Price, string ImageUrl, string Sku ,Guid CategoryId);
public record UpdateProductModel(Guid ProductId,string Name, string Description, string Currency, double Price, string ImageUrl, string Sku, Guid CategoryId);
