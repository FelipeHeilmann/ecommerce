using Domain.Categories.Entity;

namespace Infra.Models.Categories;

public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public CategoryModel(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public CategoryModel() { }

    public static CategoryModel FromAggregate(Category category)
    {
        return new CategoryModel(category.Id, category.Name, category.Description); 
    }

    public Category ToAggregate()
    {
        return new Category(Id, Name, Description);
    }
}
