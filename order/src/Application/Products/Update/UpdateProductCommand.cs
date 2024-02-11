using Application.Abstractions.Messaging;
using Application.Products.Model;

namespace Application.Products.Update;

public record UpdateProductCommand(UpdateProductModel request) : ICommand;
