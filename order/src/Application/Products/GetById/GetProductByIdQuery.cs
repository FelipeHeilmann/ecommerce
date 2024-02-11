using Application.Abstractions;
using Domain.Products;

namespace Application.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Product>;
