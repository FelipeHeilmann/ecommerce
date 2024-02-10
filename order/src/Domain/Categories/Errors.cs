using Domain.Shared;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static Error CategoryNotFound => Error.NotFound("Category.Not.Found", "The category was not found");
}
