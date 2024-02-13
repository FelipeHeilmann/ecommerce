using Application.Abstractions.Messaging;
using Application.Categories.Model;

namespace Application.Categories.Update;

public record class UpdateCategoryCommand(UpdateCategoryRequest request) : ICommand;
