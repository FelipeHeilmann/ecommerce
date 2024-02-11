using Application.Abstractions;
using Application.Products.Model;
using Domain.Products;
using Domain.Shared;

namespace Application.Products.Update;

public record UpdateProductCommand(UpdateProductModel request) : ICommand<Result<Product>>;
