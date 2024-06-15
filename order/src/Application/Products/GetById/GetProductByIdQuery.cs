using Application.Abstractions.Messaging;
using Domain.Products;

namespace Application.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Output>;
public record Output(Guid Id, string Name, string Description, string ImageUrl, double Price, string Currency, string Sku, CategoryOutput? Category);
public record CategoryOutput(Guid Id, string Name, string Descrption);