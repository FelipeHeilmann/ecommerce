using Application.Abstractions.Messaging;

namespace Application.Categories.Create;

public record CreateCategoryCommand(string Name, string Description) : ICommand<Guid>;
