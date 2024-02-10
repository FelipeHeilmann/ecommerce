using Application.Orders.Command;
using Application.Orders.Command.Cancel;
using Application.Orders.Command.RemoveItem;
using Application.Orders.Model;
using Application.Orders.Query.GetByCustomerId;
using Application.Orders.Query.GetById;
using Application.Orders.Query.GetCart;
using Domain.Orders;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;
namespace Integration;

public class OrderTest
{
    private readonly OrderRepositoryMemory _orderRepository = new();
    private readonly CustomerRepositoryMemory _customerRepository = new();
    private readonly ProductRepositoryMemory _productRepository = new();
    private readonly UnitOfWorkMemory _unitOfWork = new();

    public OrderTest()
    {
        RepositorySetup.PopulateProductRepository(_productRepository);
        RepositorySetup.PopulateCustomerRepository(_customerRepository);
        RepositorySetup.PopulateOrderRepository(_orderRepository);
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

        var commandHandler = new CreateOrderCommandHandler(_orderRepository, _productRepository, _unitOfWork);

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

        var commandHandler = new RemoveLineItemCommandHandler(_orderRepository, _unitOfWork);

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
        var commandHandler = new CancelOrderCommandHandler(_orderRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(OrderStatus.Canceled, result.Data.Status);
    }
}
