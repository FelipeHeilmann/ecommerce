using Application.Abstractions.Messaging;
using Domain.Products;

namespace Application.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Product>;
