namespace API.Requests;

public record CreateProductRequest(string Name, string Description, string Currency, double Price, string ImageUrl, string Sku ,Guid? CategoryId);
public record UpdateProductRequest(Guid ProductId,string Name, string Description, string Currency, double Price, string ImageUrl, string Sku, Guid CategoryId);
