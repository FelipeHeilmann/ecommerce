using Domain.Shared;

namespace Domain.Categories.Error;

public static class CategoryErrors
{
    public static Shared.Error CategoryNotFound => Shared.Error.NotFound("Category.Not.Found", "The category was not found");
    public static Shared.Error CategoryNameEmpty => Shared.Error.Validation("Category.Name.Empty", "The category name shoud not be empty");
    public static Shared.Error CategoryDescriptionEmpty => Shared.Error.Validation("Category.Description.Empty", "The category description shoud not be empty");
}

