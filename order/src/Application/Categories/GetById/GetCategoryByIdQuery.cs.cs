using Application.Abstractions;
using Domain.Categories;

namespace Application.Categories.GetById;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Category>;
