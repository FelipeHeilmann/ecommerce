using Application.Abstractions.Messaging;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.GetAll;

public record GetAllProductsQuery() : IQuery<ICollection<Product>>;
