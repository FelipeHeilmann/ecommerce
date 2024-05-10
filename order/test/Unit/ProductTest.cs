using Domain.Categories;
using Domain.Categories.Entity;
using Domain.Products.Entity;
using Domain.Products.Error;
using Xunit;

namespace Unit;

public class ProductTest
{
    [Fact]
    public void Should_Create_Product()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";
        var sku = "sku";

        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product = Product.Create(name, description, imageUrl, currency, price, sku, category);

        Assert.Equal(name, product.Name);

    }

    [Fact]
    public void Should_Not_Create_Product_Due_Sku_Length()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";
        var sku = "sfdbjsdfgvbsdfghsdhfsohfsohjflshfodshfosf";

        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        Assert.Throws<InvalidSku>(() => Product.Create(name, description, imageUrl, currency, price, null, category));
    }

    [Fact]
    public void Should_Not_Create_Product_Due_Sku_Null()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";

        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        Assert.Throws<InvalidSku>(() => Product.Create(name, description, imageUrl, currency, price, null, category));
    }

    [Fact]
    public void Should_Not_Create_Product_Due_Sku_Empty_String()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";

        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");

        Assert.Throws<InvalidSku>(() => Product.Create(name, description, imageUrl, currency, price, null, category));
    }
}