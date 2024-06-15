using Domain.Shared;

namespace Domain.Categories.Error;

public class InvalidCategoryName : BaseException
{
    public InvalidCategoryName(): base("Invalid.Category.Name", "Invalid category name", 400){}
}

public class InvalidCategoryDescription : BaseException
{
    public InvalidCategoryDescription(): base("Invalid.Category.Description", "Invalid category description", 400){}
}