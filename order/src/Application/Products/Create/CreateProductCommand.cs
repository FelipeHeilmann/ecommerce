using Application.Abstractions.Messaging;

namespace Application.Products.Create;

public record CreateProductCommand(string Name, string Description, string Currency, double Price, string ImageUrl, string Sku, Guid CategoryId) : ICommand<Guid>;