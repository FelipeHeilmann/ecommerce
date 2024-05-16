using Domain.Products.Entity;
using Domain.Products.VO;

namespace Infra.Models;

public class ProductsModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Currency { get; set; }
    public double Amount { get; set; }
    public string Sku { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryModel Category { get; set; }

    public ProductsModel(Guid id, string name, string description, string imageUrl, DateTime createdAt, string currency, double amount, string sku, CategoryModel category)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
        Currency = currency;
        Amount = amount;
        Sku = sku;
        Category = category;
        CategoryId = category.Id;
    }

    public ProductsModel() { }

    public static ProductsModel FromAggregate(Product product)
    {
        return new ProductsModel(product.Id, product.Name, product.Description, product.ImageUrl, product.CreatedAt, product.Price.Currency, product.Price.Amount, product.Sku.Value, CategoryModel.FromAggregate(product.Category));
    }

    public Product ToAggregate()
    {
        return new Product(Id, Name, Description, ImageUrl, CreatedAt, new Money(Currency, Amount), new Sku(Sku), CategoryId, Category.ToAggregate());
    }
}
