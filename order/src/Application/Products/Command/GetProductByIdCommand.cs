using Application.Abstractions;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Command;

public record GetProductByIdCommand(Guid ProductId) : ICommand<Result<Product>>;
