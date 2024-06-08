using Domain.Categories.Entity;
using Domain.Customers.Entity;
using Domain.Orders.Entity;
using Domain.Products.Entity;
using Xunit;

namespace Unit;

public class OrderTest
{
    [Fact]
    public void Should_Create_Order_And_Calculate_Total()
    {
        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category);
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category);
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category);

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha" ,new DateOnly(2004, 6, 11), "460.200.040-15", "11 97414-6507");

        var order = Order.Create(customer.Id, null);

        order.AddItem(product1.Id, product1.Price, 2);
        order.AddItem(product2.Id, product2.Price, 1);
        order.AddItem(product3.Id, product3.Price, 3);

        Assert.Equal((50 * 2) + (60 * 1) + (70 * 3), order.CalculateTotal());
        Assert.Equal(3, order.Items.Count());
        Assert.Equal(6, order.CountItens());
    }

    [Fact]
    public void Should_Create_Order_Remove_One_Item_And_Calculate_Total()
    {
        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category);
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category);
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category);

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateOnly(2004, 6, 11), "460.200.040-15", "11 97414-6507");

        var order = Order.Create(customer.Id, null);

        order.AddItem(product1.Id, product1.Price ,2);
        order.AddItem(product2.Id, product2.Price, 1);
        order.AddItem(product3.Id, product3.Price, 3);

        var lineItemToRemove = order.Items.FirstOrDefault(li => li.ProductId == product3.Id);

        order.RemoveItem(lineItemToRemove.Id);

        Assert.Equal(300, order.CalculateTotal());
        Assert.Equal(3, order.Items.Count());
        Assert.Equal(5, order.CountItens());
    }

    [Fact]
    public void Should_Create_Checkout_For_Order()
    {
        var category = new Domain.Categories.Entity.Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category);
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category);
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category);

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateOnly(2004, 6, 11), "460.200.040-15", "11 97414-6507");

        var order = Order.Create(customer.Id, null);

        order.AddItem(product1.Id, product1.Price, 2);
        order.AddItem(product2.Id, product2.Price, 1);
        order.AddItem(product3.Id, product3.Price, 3);

        order.Checkout(Guid.NewGuid(), Guid.NewGuid(), "credit", "token", 10);

        Assert.Equal("waiting_payment", order.Status);
    }
}
