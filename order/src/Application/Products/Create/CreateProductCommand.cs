using Application.Abstractions.Messaging;
using Application.Products.Model;

namespace Application.Products.Create;

public record CreateProductCommand(CreateProductRequest request) : ICommand<Guid>;