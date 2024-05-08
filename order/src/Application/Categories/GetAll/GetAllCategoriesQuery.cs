using Application.Abstractions.Messaging;

namespace Application.Categories.GetAll;

public record GetAllCategoriesQuery : IQuery<ICollection<Output>>;
public record Output(Guid Id, string Name, string Descrption);
