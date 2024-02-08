using Application.Abstractions;
using Application.Products.Model;
using Domain.Shared;

namespace Application.Products.Command;

public record CreateProductCommand(CreateProductModel request) : ICommand<Result<Guid>>;
