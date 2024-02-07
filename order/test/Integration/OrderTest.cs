using Application.Orders.Command;
using Application.Orders.Model;
using Domain.Customer;
using Domain.Products;
using Infra.Repositories;
using Xunit;
namespace Integration;

public class OrderTest
{
    private readonly OrderRepositoryMemory _orderRepository = new();
    private readonly CustomerRepositoryMemory _customerRepository = new();
    private readonly ProductRepositoryMemory _productRepository = new();

    public OrderTest() 
    {
        _productRepository.Add(new Product(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
        _productRepository.Add(new Product(Guid.Parse("6021dc49-a9f5-43bb-9602-c1689c5549e3"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 70.00), Sku.Create("0002"), Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), new Category(Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), "categoria nome", "categoria descricao")));
        _productRepository.Add(new Product(Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 60.00), Sku.Create("0003"), Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), "categoria nome", "categoria descricao")));
        _customerRepository.Add(new Customer(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), Name.Create("Felipe Heilmann").Data, Email.Create("felipeheilmannm@gmail.com").Data, new DateTime(2004, 6, 11), DateTime.Now));
    }

    [Fact]
    public async Task Should_Create_Order_With_3_Itens()
    {
        var customerId = Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0");
        var requestItensList = new List<OrderItemRequestModel>()
        {
            new OrderItemRequestModel(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), 2),
            new OrderItemRequestModel(Guid.Parse("6021dc49-a9f5-43bb-9602-c1689c5549e3"), 3),
            new OrderItemRequestModel(Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), 4)
        };
        var request = new OrderRequestModel(requestItensList, customerId);

        var command = new CreateOrderCommand(request);

        var commandHandler = new CreateOrderCommandHandler(_orderRepository, _productRepository);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        var order = result.Data;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal((50 * 2) + (70 * 3) + (60 * 4), order.CalculateTotal());
    }
}
