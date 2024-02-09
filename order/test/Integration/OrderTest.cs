using Application.Orders.Command;
using Application.Orders.Command.Cancel;
using Application.Orders.Command.RemoveItem;
using Application.Orders.Model;
using Application.Orders.Query.GetByCustomerId;
using Application.Orders.Query.GetById;
using Application.Orders.Query.GetCart;
using Domain.Customer;
using Domain.Orders;
using Domain.Products;
using Infra.Repositories.Memory;
using Xunit;
namespace Integration;

public class OrderTest
{
    private readonly OrderRepositoryMemory _orderRepository = new();
    private readonly CustomerRepositoryMemory _customerRepository = new();
    private readonly ProductRepositoryMemory _productRepository = new();

    public OrderTest() 
    {
        _customerRepository.Add(new Customer(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), Name.Create("Felipe Heilmann").Data, Email.Create("felipeheilmannm@gmail.com").Data, new DateTime(2004, 6, 11), DateTime.Now));

        _productRepository.Add(new Product(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 50.00), Sku.Create("0001"), Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), new Category(Guid.Parse("a7efe841-8e19-4fe8-afcd-e3742a3dacf4"), "categoria nome", "categoria descricao")));
        _productRepository.Add(new Product(Guid.Parse("6021dc49-a9f5-43bb-9602-c1689c5549e3"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 70.00), Sku.Create("0002"), Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), new Category(Guid.Parse("538f1d08-9f31-4058-9538-55f388cde724"), "categoria nome", "categoria descricao")));
        _productRepository.Add(new Product(Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 60.00), Sku.Create("0003"), Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), "categoria nome", "categoria descricao")));
        
        _productRepository.Add(new Product(Guid.Parse("9d9d284f-6b19-4b34-9e29-45c6d8f45bfa"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 60.00), Sku.Create("0003"), Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), "categoria nome", "categoria descricao")));
        _productRepository.Add(new Product(Guid.Parse("20caccf1-6a2b-40fb-b9da-1efbca3f734f"), "Nome do produto", "Desricao", "Imagem", new Money("BRL", 100.00), Sku.Create("0003"), Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), new Category(Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), "categoria nome", "categoria descricao")));
        var order = new Order(Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"), Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0"), OrderStatus.Created, DateTime.Now, DateTime.Now);
        var lineItens = new List<LineItem>()
        {
            new LineItem(Guid.Parse("efd7d188-b573-46ba-aa2f-6fd139d1813a"), Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"), Guid.Parse("9d9d284f-6b19-4b34-9e29-45c6d8f45bfa"), new Money("BRL", 60.00), 2),
            new LineItem(Guid.Parse("27046ada-aee4-4325-8bc5-2affe5cf9627"), Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f"), Guid.Parse("20caccf1-6a2b-40fb-b9da-1efbca3f734f"), new Money("BRL", 100.00), 1)
        };

        order.RestoreLineItens(lineItens);

        _orderRepository.Add(order);
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

    [Fact]
    public async Task Should_Get_Order_By_Id()
    {
        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");
       
        var query = new GetOrderByIdQuery(orderId);

        var queryHandler = new GetOrderByIdQueryHandler(_orderRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        var order = result.Data;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(orderId, order.Id);
    }

    [Fact]
    public async Task Should_Not_Get_Order_By_Id()
    {
        var orderId = Guid.NewGuid();

        var query = new GetOrderByIdQuery(orderId);

        var commandHandler = new GetOrderByIdQueryHandler(_orderRepository);

        var result = await commandHandler.Handle(query, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Should_Remove_Line_Item_From_Order()
    {
        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");
        var lineItemId = Guid.Parse("efd7d188-b573-46ba-aa2f-6fd139d1813a");

        var command = new RemoveLineItemCommand(orderId,lineItemId);

        var commandHandler = new RemoveLineItemCommandHandler(_orderRepository);

        var result = await commandHandler.Handle(command, CancellationToken.None);
        
        var order = result.Data;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(60 + 100, order.CalculateTotal());
    }

    [Fact]
    public async Task Should_Get_Orders_By_CustomerId()
    {
        var customerId = Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0");

        var query = new GetOrdersByCustomerQuery(customerId);

        var queryHandler = new GetOrdersByCustomerIdQueryHandler(_orderRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(1, result.Data.Count);
    }

    [Fact]
    public async Task Should_Get_Cart()
    {
        var query = new GetCartQuery();
        var querHandler = new GetCartQueryHandler(_orderRepository);

        var result = await querHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(OrderStatus.Created, result.Data.Status);
    }

    [Fact]
    public async Task Should_Cancel_Order()
    {
        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");
        var command = new CancelOrderCommand(orderId);
        var commandHandler = new CancelOrderCommandHandler(_orderRepository);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(OrderStatus.Canceled, result.Data.Status);
    }
}
