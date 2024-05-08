using Application.Abstractions.Messaging;

namespace Application.Products.GetAll;

public record GetAllProductsQuery() : IQuery<ICollection<Output>>;
public record Output(Guid Id, string Name, string Description, string ImageUrl, double Price, string Currency, string Sku, CategoryOutput Category);
public record CategoryOutput(Guid Id, string Name, string Descrption);
