﻿using Application.Orders.Create;
using Application.Orders.Cancel;
using Application.Orders.RemoveItem;
using Application.Orders.Model;
using Application.Orders.GetByCustomerId;
using Application.Orders.GetById;
using Application.Orders.GetCart;
using Infra.Data;
using Infra.Repositories.Memory;
using Xunit;
using Application.Orders.AddLineItem;
using Application.Data;
using Application.Orders.Checkout;
using Application.Abstractions.Queue;
using Infra.Queue;
using Moq;
using MediatR;
using Domain.Orders.VO;
using Domain.Orders.Events;
using Domain.Orders.Repository;
using Domain.Addresses.Repository;
using Domain.Customers.Repository;
using Domain.Products.Repository;

namespace Integration;

public class OrderTest
{
    private readonly IOrderRepository _orderRepository = new OrderRepositoryMemory();
    private readonly ICustomerRepository _customerRepository = new CustomerRepositoryMemory();
    private readonly IProductRepository _productRepository = new ProductRepositoryMemory();
    private readonly IQueue _queue = new MemoryMQAdapter();
    private readonly IAddressRepository _addressRepository = new AddressRepositoryInMemory();
    private readonly IUnitOfWork _unitOfWork = new UnitOfWorkMemory();

    public OrderTest()
    {
        RepositorySetup.PopulateProductRepository(_productRepository);
        RepositorySetup.PopulateCustomerRepository(_customerRepository);
        RepositorySetup.PopulateOrderRepository(_orderRepository);
        RepositorySetup.PopulateAddressRepository(_addressRepository);  
    }

    [Fact]
    public async Task Should_Create_Order_With_3_Itens()
    {
        var customerId = Guid.Parse("f3b205c3-552d-4fd9-b10e-6414086910b0");

        var requestItensList = new List<OrderItemRequest>()
        {
            new OrderItemRequest(Guid.Parse("55b86726-d9fb-4745-b64a-66923b584cf2"), 2),
            new OrderItemRequest(Guid.Parse("6021dc49-a9f5-43bb-9602-c1689c5549e3"), 3),
            new OrderItemRequest(Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), 4)
        };
        var request = new OrderRequest(requestItensList, customerId);

        var command = new CreateOrderCommand(request);

        var mediatorMock = new Mock<IMediator>();

        mediatorMock.Setup(m => m.Publish(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var commandHandler = new CreateOrderCommandHandler(_orderRepository, _productRepository, _unitOfWork, mediatorMock.Object, _customerRepository);

        await commandHandler.Handle(command, CancellationToken.None);

        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");

        var query = new GetOrderByIdQuery(orderId);

        var queryHandler = new GetOrderByIdQueryHandler(_orderRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        var order = result.Value;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }


    [Fact]
    public async Task Should_Add_One_Item_To_Order()
    {
   
        var command = new AddLineItemCommand( Guid.NewGuid() ,Guid.Parse("cb67d960-af04-40c8-92da-9d4ff28da6f8"), 4);

        var commandHandler = new AddLineItemCommandHandler(_orderRepository, _productRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public async Task Should_Get_Order_By_Id()
    {
        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");
       
        var query = new GetOrderByIdQuery(orderId);

        var queryHandler = new GetOrderByIdQueryHandler(_orderRepository);

        var result = await queryHandler.Handle(query, CancellationToken.None);

        var order = result.Value;

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

        var command = new RemoveLineItemCommand(lineItemId);

        var commandHandler = new RemoveLineItemCommandHandler(_orderRepository, _unitOfWork);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        var query = new GetOrderByIdQuery(orderId);

        var queryHandler = new GetOrderByIdQueryHandler(_orderRepository);

        var resultGetOrder = await queryHandler.Handle(query, CancellationToken.None);

        var order = resultGetOrder.Value;

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(60 + 100, order.Total);
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
        Assert.Equal(2, result.Value.Count);
    }

    [Fact]
    public async Task Should_Get_Cart()
    {
        var query = new GetCartQuery();
        var querHandler = new GetCartQueryHandler(_orderRepository);

        var result = await querHandler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("created", result.Value.Status);
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
    }

    [Fact]
    public async Task Should_Create_Checkout_For_Order()
    {
        var orderId = Guid.Parse("c3a9083c-a259-4516-8842-a80b40f8c39f");

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var command = new CheckoutOrderCommand(
                orderId,
                Guid.Parse("2b169c76-acee-4ddf-86c4-37af9fbb07ea"),
                Guid.Parse("2b169c76-acee-4ddf-86c4-37af9fbb07ea"),
                paymentType,
                cardToken,
                installments
            );

        var commandHandler = new CheckoutOrderCommandHandler(_orderRepository, _customerRepository ,_addressRepository, _unitOfWork, _queue);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }
}
