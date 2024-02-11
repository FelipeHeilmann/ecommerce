using Application.Abstractions;
using Domain.Shared;

namespace Application.Products.Delete;

public record DeleteProductCommand(Guid ProductId) : ICommand<Result>;
