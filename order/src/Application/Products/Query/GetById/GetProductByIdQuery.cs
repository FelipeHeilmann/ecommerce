using Application.Abstractions;
using Domain.Products;

namespace Application.Products.Query;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Product>;
