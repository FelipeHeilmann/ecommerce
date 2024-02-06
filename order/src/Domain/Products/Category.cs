namespace Domain.Products;

public record Category(Guid Id, string Name, string Description, Guid productId);
