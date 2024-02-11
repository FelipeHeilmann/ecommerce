using Application.Abstractions.Messaging;
using Domain.Categories;

namespace Application.Categories.GetById;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Category>;
