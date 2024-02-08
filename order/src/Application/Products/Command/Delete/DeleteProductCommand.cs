using Application.Abstractions;
using Domain.Shared;

namespace Application.Products.Command;

public record DeleteProductCommand(Guid ProductId) : ICommand<Result>;
