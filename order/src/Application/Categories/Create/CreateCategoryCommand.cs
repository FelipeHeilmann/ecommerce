using Application.Abstractions.Messaging;
using Application.Categories.Model;
using Domain.Categories;

namespace Application.Categories.Create;

public record CreateCategoryCommand(CreateCategoryRequest request) : ICommand<Guid>;
