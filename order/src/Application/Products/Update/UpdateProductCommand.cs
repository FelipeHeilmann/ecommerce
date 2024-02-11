using Application.Abstractions;
using Application.Products.Model;

namespace Application.Products.Update;

public record UpdateProductCommand(UpdateProductModel request) : ICommand;
