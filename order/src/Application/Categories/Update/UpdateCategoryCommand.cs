using Application.Abstractions;
using Application.Categories.Model;

namespace Application.Categories.Update;

public record class UpdateCategoryCommand(UpdateCategoryModel request) : ICommand;
