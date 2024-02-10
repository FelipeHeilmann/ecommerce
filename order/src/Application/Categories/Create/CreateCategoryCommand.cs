using Application.Abstractions;
using Application.Categories.Model;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.Create;

public record CreateCategoryCommand(CategoryModel request) : ICommand<Result<Category>>;
