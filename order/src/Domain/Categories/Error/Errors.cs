using Domain.Shared;

namespace Domain.Categories.Error;

public static class CategoryErrors
{
    public static Error CategoryNotFound => Error.NotFound("Category.Not.Found", "The category was not found");
    public static Error CategoryNameEmpty => Error.Validation("Category.Name.Empty", "The category name shoud not be empty");
    public static Error CategoryDescriptionEmpty => Error.Validation("Category.Description.Empty", "The category description shoud not be empty");
}

