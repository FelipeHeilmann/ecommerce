using Domain.Categories.Entity;
using Domain.Products.VO;
using Domain.Shared;

namespace Domain.Products.Entity;

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

    public Product(Guid id, string name, string description, string imageUrl, DateTime createdAt, Money money, Sku sku, Guid categoryId, Category category)
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

    public static Product Create(string name, string description, string imageUrl, string currency, double price, string skuString, Category category)
    {
        var money = new Money(currency, price);

        return new Product(Guid.NewGuid(), name, description, imageUrl, DateTime.UtcNow, money, new Sku(skuString), category.Id, category);
    }

    public void Update(string name, string description, string imageUrl, string currency, double price, string skuString, Category category)
    {
        var money = new Money(currency, price);
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CategoryId = Category.Id;
        Category = category;
        Sku = new Sku(skuString);
        Price = money;
    }
}