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
using Application.Customers.Create;
using Application.Customers.Model;
using Domain.Customers.Event;
using Infra.Implementations;
using Application.Abstractions.Services;
using Application.Products.Create;
using Application.Products.Model;
using Domain.Categories.Repository;
using Application.Categories.Model;
using Application.Categories.Create;
using Application.Addresses.Model;
using Application.Addresses.Create;

namespace Integration;

public class OrderTest
{
    private readonly IOrderRepository orderRepository = new OrderRepositoryMemory();
    private readonly ICustomerRepository customerRepository = new CustomerRepositoryMemory();
    private readonly IProductRepository productRepository = new ProductRepositoryMemory();
    private readonly IAddressRepository addressRepository = new AddressRepositoryInMemory();
    private readonly ICategoryRepository categoryRepository = new CategoryRepositoryMemory();
    private readonly IPasswordHasher passwordHasher = new PasswordHasher();
    private readonly IQueue queue = new MemoryMQAdapter();
    private readonly IUnitOfWork unitOfWork = new UnitOfWorkMemory();


    [Fact]
    public async Task Should_Create_Order_With_3_Items()
    {
        //GIVEN
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mediator.Setup(m => m.Publish(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507"); 

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);
       
        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct2), CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct3), CancellationToken.None);

        var inputCreateOrder = new OrderRequest(new List<OrderItemRequest>()
        {
            new OrderItemRequest(outputCreateProduct1.Value, 2),
            new OrderItemRequest(outputCreateProduct2.Value, 3),
            new OrderItemRequest(outputCreateProduct3.Value, 4)
        }, outputCreateCustomer.Value);

        var createOrdercommandHandler = new CreateOrderCommandHandler(orderRepository, productRepository, unitOfWork, mediator.Object, customerRepository);

        //WHEN
        var outputCreateOrder = await createOrdercommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);

        var getOrderQuery = new GetOrderByIdQuery(outputCreateOrder.Value);

        var getOrderQueryHandler = new GetOrderByIdQueryHandler(orderRepository);

        var outputGetOrder = await getOrderQueryHandler.Handle(getOrderQuery, CancellationToken.None);

        var order = outputGetOrder.Value;

        //THEN
        Assert.Equal(3,order.Items.Count());
        Assert.Equal(2, order.Items.ToList()[0].Quantity);
        Assert.Equal(3, order.Items.ToList()[1].Quantity);
        Assert.Equal(4, order.Items.ToList()[2].Quantity);
    }


    [Fact]
    public async Task Should_Create_Cart_And_Add_One_Item()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);

        var addItemToCartCommandHandler = new AddItemToCartCommandHandler(orderRepository, productRepository, unitOfWork);

        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct1.Value, 4), CancellationToken.None);

        var getCartQueryHandler = new GetCartQueryHandler(orderRepository);

        var outputGetCart = await getCartQueryHandler.Handle(new GetCartQuery(), CancellationToken.None);

        Assert.Equal(1, outputGetCart.Value?.Items?.Count());
        Assert.Equal("cart", outputGetCart?.Value?.Status);
    }


    [Fact]
    public async Task Should_Create_Cart_Add_3_Items_And_Remove_One()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct2), CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct3), CancellationToken.None);

        var addItemToCartCommandHandler = new AddItemToCartCommandHandler(orderRepository, productRepository, unitOfWork);

        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct1.Value, 4), CancellationToken.None);
        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct2.Value, 4), CancellationToken.None);
        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct3.Value, 1), CancellationToken.None);

        var getCartQueryHandler = new GetCartQueryHandler(orderRepository);

        var outputGetCartBeforeRemove = await getCartQueryHandler.Handle(new GetCartQuery(), CancellationToken.None);

        var lineItemToRemove = outputGetCartBeforeRemove.Value.Items?.FirstOrDefault(li => li.ProductId == outputCreateProduct3.Value);

        var removeLineItemFromCartCommandHandler = new RemoveItemFromCartCommandHandler(orderRepository, unitOfWork);

        await removeLineItemFromCartCommandHandler.Handle(new RemoveItemFromCartCommand(lineItemToRemove.LineItemId), CancellationToken.None);

        var outputGetCartAfterRemove = await getCartQueryHandler.Handle(new GetCartQuery(), CancellationToken.None);

        Assert.Equal(2, outputGetCartAfterRemove.Value?.Items?.Count());
        Assert.Equal("cart", outputGetCartAfterRemove.Value?.Status);
    }

    [Fact]
    public async Task Should_Get_Orders_By_CustomerId()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mediator.Setup(m => m.Publish(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct2), CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct3), CancellationToken.None);

        var inputCreateOrder = new OrderRequest(new List<OrderItemRequest>()
        {
            new OrderItemRequest(outputCreateProduct1.Value, 2),
            new OrderItemRequest(outputCreateProduct2.Value, 3),
            new OrderItemRequest(outputCreateProduct3.Value, 4)
        }, outputCreateCustomer.Value);

        var createOrdercommandHandler = new CreateOrderCommandHandler(orderRepository, productRepository, unitOfWork, mediator.Object, customerRepository);

        //WHEN
        await createOrdercommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);
        await createOrdercommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);
        await createOrdercommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);

        var getOrdersByCustomerIdQueryHandler = new GetOrdersByCustomerIdQueryHandler(orderRepository);

        var outputGetOrders = await getOrdersByCustomerIdQueryHandler.Handle(new GetOrdersByCustomerIdQuery(outputCreateCustomer.Value), CancellationToken.None);

        Assert.Equal(3, outputGetOrders.Value.Count());
    }


    [Fact]
    public async Task Should_Cancel_Order()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mediator.Setup(m => m.Publish(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct2), CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct3), CancellationToken.None);

        var inputCreateOrder = new OrderRequest(new List<OrderItemRequest>()
        {
            new OrderItemRequest(outputCreateProduct1.Value, 2),
            new OrderItemRequest(outputCreateProduct2.Value, 3),
            new OrderItemRequest(outputCreateProduct3.Value, 4)
        }, outputCreateCustomer.Value);

        var createOrderCommandHandler = new CreateOrderCommandHandler(orderRepository, productRepository, unitOfWork, mediator.Object, customerRepository);

        //WHEN
        var outputCreateOrder = await createOrderCommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);

        var cancelOrderCommandHandler = new CancelOrderCommandHandler(orderRepository, unitOfWork);

        await cancelOrderCommandHandler.Handle(new CancelOrderCommand(outputCreateOrder.Value), CancellationToken.None);

        var getOrder = new GetOrderByIdQueryHandler(orderRepository);

        var outputGetOrder = await getOrder.Handle(new GetOrderByIdQuery(outputCreateOrder.Value), CancellationToken.None);

        Assert.Equal("canceled", outputGetOrder.Value.Status);
    }

    [Fact]
    public async Task Should_Create_Checkout_For_Order()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Publish(It.IsAny<CustomerCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mediator.Setup(m => m.Publish(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var inputCreateCustomer = new CreateCustomerRequest("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateAccountCommandHandler(customerRepository, unitOfWork, passwordHasher, mediator.Object);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(new CreateAccountCommand(inputCreateCustomer), CancellationToken.None);

        var inputCreateCategory = new CreateCategoryRequest("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository, unitOfWork);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(new CreateCategoryCommand(inputCreateCategory), CancellationToken.None);

        var inputCreateProduct1 = new CreateProductRequest("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductRequest("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductRequest("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository, unitOfWork);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct1), CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct2), CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(new CreateProductCommand(inputCreateProduct3), CancellationToken.None);

        var inputCreateOrder = new OrderRequest(new List<OrderItemRequest>()
        {
            new OrderItemRequest(outputCreateProduct1.Value, 2),
            new OrderItemRequest(outputCreateProduct2.Value, 3),
            new OrderItemRequest(outputCreateProduct3.Value, 4)
        }, outputCreateCustomer.Value);

        var createOrderCommandHandler = new CreateOrderCommandHandler(orderRepository, productRepository, unitOfWork, mediator.Object, customerRepository);

        //WHEN
        var outputCreateOrder = await createOrderCommandHandler.Handle(new CreateOrderCommand(inputCreateOrder), CancellationToken.None);

        var inputCreateAddress = new CreateAddressRequest(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository, unitOfWork);

        var outputCreateAddress = await createAddressCommandHandler.Handle(new CreateAddressCommand(inputCreateAddress), CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository ,addressRepository, unitOfWork, queue);

        await checkoutOrderCommandHandler.Handle(new CheckoutOrderCommand(
                outputCreateOrder.Value,
                outputCreateAddress.Value,
                outputCreateAddress.Value,
                paymentType,
                cardToken,
                installments
            ), CancellationToken.None);

        var getOrder = new GetOrderByIdQueryHandler(orderRepository);

        var outputGetOrder = await getOrder.Handle(new GetOrderByIdQuery(outputCreateOrder.Value), CancellationToken.None);

        Assert.Equal("waiting_payment", outputGetOrder.Value.Status);
    }
}
