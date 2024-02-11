using Application.Abstractions.Messaging;
using Domain.Categories;

namespace Application.Categories.GetAll;

public record GetAllCategoriesQuery : IQuery<ICollection<Category>>;
