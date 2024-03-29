﻿using Domain.Categories;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using Xunit;

namespace Unit;

public class OrderTest
{
    [Fact]
    public void Should_Create_Order_And_Calculate_Total()
    {
        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category).Value;
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category).Value;
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category).Value;

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha" ,new DateOnly(2004, 6, 11), "44444444444", "11 97414-6507");

        var order = Order.Create(customer.Value.Id);

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
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category).Value;
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category).Value;
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category).Value;

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateOnly(2004, 6, 11), "44444444444", "11 97414-6507");

        var order = Order.Create(customer.Value.Id);

        var lineItem1 = order.AddItem(product1.Id, product1.Price ,2);
        var lineItem2 = order.AddItem(product2.Id, product2.Price, 1);
        var lineItem3 = order.AddItem(product3.Id, product3.Price, 3);

        order.RemoveItem(lineItem3.Id);

        Assert.Equal(300, order.CalculateTotal());
        Assert.Equal(3, order.Items.Count());
        Assert.Equal(5, order.CountItens());
    }

    [Fact]
    public void Should_Create_Checkout_For_Order()
    {
        var category = new Category(Guid.NewGuid(), "categoria nome", "categoria descricao");
        var product1 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 50.0, "0001", category).Value;
        var product2 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 60.0, "0002", category).Value;
        var product3 = Product.Create("Nome do produto", "Desricao", "Imagem", "BRL", 70.0, "0003", category).Value;

        var customer = Customer.Create("Felipe Heilmann", "felipeheilmannm@gmail.com", "senha", new DateOnly(2004, 6, 11), "44444444444", "11 97414-6507");

        var order = Order.Create(customer.Value.Id);

        order.AddItem(product1.Id, product1.Price, 2);
        order.AddItem(product2.Id, product2.Price, 1);
        order.AddItem(product3.Id, product3.Price, 3);

        order.Checkout(Guid.NewGuid(), Guid.NewGuid());

        Assert.Equal(OrderStatus.WaitingPayment, order.Status);
    }
}
