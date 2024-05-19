using Application.Abstractions.Messaging;

namespace Application.Categories.Update;

public record class UpdateCategoryCommand(string Name, string Description, Guid CategoryId) : ICommand;
