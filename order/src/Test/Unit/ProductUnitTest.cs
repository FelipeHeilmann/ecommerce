using Domain.Products;
using Xunit;

namespace Test.Unit;

public class ProductUnitTest
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

        var product = Product.Create(name, description, imageUrl, currency, price,sku);

        Assert.False(product.IsFailure);
        Assert.True(product.IsSuccess);
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

        var product = Product.Create(name, description, imageUrl, currency, price, sku);

        Assert.True(product.IsFailure);
        Assert.False(product.IsSuccess);
    }

    [Fact]
    public void Should_Not_Create_Product_Due_Sku_Null()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";

        var product = Product.Create(name, description, imageUrl, currency, price, null);

        Assert.True(product.IsFailure);
        Assert.False(product.IsSuccess);
    }

    [Fact]
    public void Should_Not_Create_Product_Due_Sku_Empty_String()
    {
        var name = "name";
        var description = "description";
        var currency = "BRL";
        var price = 200.0;
        var imageUrl = "image";

        var product = Product.Create(name, description, imageUrl, currency, price, "");

        Assert.True(product.IsFailure);
        Assert.False(product.IsSuccess);
    }
}

