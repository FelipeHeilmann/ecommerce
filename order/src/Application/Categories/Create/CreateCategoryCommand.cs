using Application.Abstractions;
using Application.Categories.Model;
using Domain.Categories;

namespace Application.Categories.Create;

public record CreateCategoryCommand(CategoryModel request) : ICommand<Guid>;
