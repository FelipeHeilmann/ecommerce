using Application.Abstractions.Messaging;

namespace Application.Categories.GetById;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Output>;
public record Output(Guid Id, string Name, string Descrption);
