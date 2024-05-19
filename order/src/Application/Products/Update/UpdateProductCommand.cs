using Application.Abstractions.Messaging;

namespace Application.Products.Update;

public record UpdateProductCommand(Guid ProductId, string Name, string Description, string Currency, double Price, string ImageUrl, string Sku, Guid CategoryId) : ICommand;
