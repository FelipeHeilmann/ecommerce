namespace Application.Categories.Model;

public record CategoryModel(string Name, string Description);


public record UpdateCategoryModel(string Name, string Description, Guid CustomerId);