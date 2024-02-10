using Application.Abstractions;
using Application.Categories.Model;
using Domain.Categories;
using Domain.Shared;

namespace Application.Categories.Update;

public record class UpdateCategoryCommand(UpdateCategoryModel request) : ICommand<Result<Category>>;
