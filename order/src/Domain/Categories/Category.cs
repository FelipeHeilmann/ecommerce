namespace Domain.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Category(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }   

    public static Category Create(string name, string description)
    {
        return new Category(Guid.NewGuid(), name, description);
    }
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
