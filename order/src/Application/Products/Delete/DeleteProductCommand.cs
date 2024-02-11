using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Products.Delete;

public record DeleteProductCommand(Guid ProductId) : ICommand;
