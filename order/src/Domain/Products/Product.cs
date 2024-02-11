using Domain.Categories;
using Domain.Shared;

namespace Domain.Products;

public class Product 
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Money Price { get; private set; }
    public Sku Sku { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } 

    public Product(Guid id, string name, string description, string imageUrl, DateTime createdAt ,Money money, Sku sku, Guid categoryId ,Category category)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        Price = money;
        Sku = sku;
        CategoryId = categoryId;
        Category = category;
        CreatedAt = createdAt;
    }

    public Product() { }

    public static Result<Product> Create(string name, string description, string imageUrl, string currency, double price, string skuString, Category category)
    {   
        var money = new Money(currency, price);
        var sku = Sku.Create(skuString);

        if (sku.IsFailure)
        {
            return Result.Failure<Product>(sku.Error);
        }

        return new Product(Guid.NewGuid(), name, description, imageUrl, DateTime.UtcNow ,money, sku.Value, category.Id ,category);
    }

    public Result<Product> Update(string name, string description, string imageUrl, string currency, double price, string skuString, Category category)
    {
        var money = new Money(currency, price);
        var sku = Sku.Create(skuString);

        if (sku.IsFailure)
        {
            return Result.Failure<Product>(sku.Error);
        }

        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CategoryId = Category.Id;
        Category = category;
        Sku = sku.Value;
        Price = money;
        
        return Result.Success(this);
    }
}