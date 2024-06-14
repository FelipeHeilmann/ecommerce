using Domain.Categories.Entity;
using Domain.Products.VO;
using Domain.Shared;

namespace Domain.Products.Entity;

public class Product
{
    private Money _price;
    private Sku _sku;
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Currency { get =>  _price.Currency; }
    public double Amount { get => _price.Amount; }
    public string Sku {get => _sku.Value; }
    public Guid? CategoryId { get; private set; }

    private Product(Guid id, string name, string description, string imageUrl, DateTime createdAt, Money money, Sku sku, Guid? categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        _price = money;
        _sku = sku;
        CategoryId = categoryId;
        CreatedAt = createdAt;
    }

    public static Product Create(string name, string description, string imageUrl, string currency, double price, string skuString, Guid? categoryId)
    {
        var money = new Money(currency, price);

        return new Product(Guid.NewGuid(), name, description, imageUrl, DateTime.UtcNow, money, new Sku(skuString), categoryId);
    }

    public static Product Restore(Guid id, string name, string description, string imageUrl, string currency, double amount, string sku, Guid? categoryId, DateTime createdAt)
    {
        return new Product(id, name, description, imageUrl, createdAt, new Money(currency, amount), new Sku(sku), categoryId);
    }

    public void Update(string name, string description, string imageUrl, string currency, double price, string skuString, Guid? categoryId)
    {
        var money = new Money(currency, price);
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        _sku = new Sku(skuString);
        _price = money;
    }
}