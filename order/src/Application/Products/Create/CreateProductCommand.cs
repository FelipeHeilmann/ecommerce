using Application.Abstractions;
using Application.Products.Model;

namespace Application.Products.Create;

public record CreateProductCommand(CreateProductModel request) : ICommand<Guid>;