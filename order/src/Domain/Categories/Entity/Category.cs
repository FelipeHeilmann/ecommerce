using Domain.Categories.Error;
using Domain.Shared;

namespace Domain.Categories.Entity;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Category Create(string name, string description)
    {
        if (string.IsNullOrEmpty(name)) throw new InvalidCategoryName();
        if (string.IsNullOrEmpty(description)) throw new InvalidCategoryDescription();
        return new Category(Guid.NewGuid(), name, description);
    }

    public static Category Restore(Guid id, string name, string description)
    {
        return new Category(id, name, description);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
