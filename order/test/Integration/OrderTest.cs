using Application.Orders.Cancel;
using Application.Orders.Model;
using Application.Orders.GetCart;
using Infra.Repositories.Memory;
using Xunit;
using Application.Orders.Checkout;
using Application.Abstractions.Queue;
using Infra.Queue;
using Domain.Orders.Repository;
using Domain.Addresses.Repository;
using Domain.Customers.Repository;
using Domain.Products.Repository;
using Application.Customers.Create;
using Infra.Implementations;
using Application.Abstractions.Services;
using Application.Products.Create;
using Domain.Categories.Repository;
using Application.Categories.Create;
using Application.Addresses.Create;
using Application.Orders.AddItemToCart;
using Application.Orders.RemoveItemRemoveItemFromCart;
using Application.Abstractions.Query;
using Infra.Context;
using Application.Orders.PrepareOrderForShipping;
using Application.Orders.OrderPaymentStatusChanged;
using Domain.Coupons.Repository;
using Application.Coupons.Create;

namespace Integration;

public class OrderTest
{
    private readonly IOrderRepository orderRepository;
    private readonly ICustomerRepository customerRepository;
    private readonly IProductRepository productRepository;
    private readonly IAddressRepository addressRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly ICouponRepository couponRepository;
    private readonly IOrderQueryContext orderQueryContext;
    private readonly IPasswordHasher passwordHasher;
    private readonly IQueue queue;

    public OrderTest()
    {
        orderRepository = new OrderRepositoryMemory();
        customerRepository = new CustomerRepositoryMemory();
        productRepository = new ProductRepositoryMemory();
        addressRepository = new AddressRepositoryMemory();
        categoryRepository = new CategoryRepositoryMemory();
        couponRepository = new CouponRepositoryMemory();
        orderQueryContext = new MemoryOrderContext();
        passwordHasher = new PasswordHasher();
        queue = new MemoryMQAdapter();
    }

    [Fact]
    public async Task Should_Create_Cart()
    {
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);

        var addItemToCartCommandHandler = new AddItemToCartCommandHandler(orderRepository, productRepository);

        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct1.Value, 4), CancellationToken.None);

        var outputGetCart = await orderRepository.GetCart(CancellationToken.None);

        Assert.Equal(1, outputGetCart?.Items.Count());
        Assert.Equal("cart", outputGetCart?.Status);
        Assert.Equal(200, outputGetCart?.CalculateTotal());
    }

    [Fact]
    public async Task Should_Create_Order()
    {
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var inputCheckout = new CheckoutOrderCommand(new List<OrderItemRequest>()
            {
                new OrderItemRequest(outputCreateProduct1.Value, 2),
                new OrderItemRequest(outputCreateProduct2.Value, 3),
                new OrderItemRequest(outputCreateProduct3.Value, 4)
            },
            outputCreateCustomer.Value,
            outputCreateAddress.Value,
            outputCreateAddress.Value,
            null,
            paymentType,
            cardToken,
            installments);

        //WHEN

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository, productRepository, addressRepository, couponRepository, queue);

        var outputCheckoutOrder = await checkoutOrderCommandHandler.Handle(inputCheckout, CancellationToken.None);

        var outputGetOrder = await orderRepository.GetByIdAsync(outputCheckoutOrder.Value, CancellationToken.None);

        Assert.Equal("waiting_payment", outputGetOrder?.Status);
        Assert.Equal(560, outputGetOrder?.CalculateTotal());
    }

    [Fact]
    public async Task Should_Create_Order_And_ApplyCoupon()
    { 
        var inputCreateCoupon = new CreateCouponCommand("CUPOM10", DateTime.Now.AddDays(10), 10.00);

        var createCouponCommandHandler = new CreateCouponCommandHandler(couponRepository);

        await createCouponCommandHandler.Handle(inputCreateCoupon, CancellationToken.None);

        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var inputCheckout = new CheckoutOrderCommand(new List<OrderItemRequest>()
        {
            new OrderItemRequest(outputCreateProduct1.Value, 2),
            new OrderItemRequest(outputCreateProduct2.Value, 3),
            new OrderItemRequest(outputCreateProduct3.Value, 4)
        },
        outputCreateCustomer.Value,
        outputCreateAddress.Value,
        outputCreateAddress.Value,
        "CUPOM10",
        paymentType,
        cardToken,
        installments);

        //WHEN

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository, productRepository, addressRepository, couponRepository, queue);

        var outputCheckoutOrder = await checkoutOrderCommandHandler.Handle(inputCheckout, CancellationToken.None);

        var outputGetOrder = await orderRepository.GetByIdAsync(outputCheckoutOrder.Value, CancellationToken.None);

        Assert.Equal("waiting_payment", outputGetOrder?.Status);
        Assert.Equal(504.00, outputGetOrder?.CalculateTotal());
    }


    [Fact]
    public async Task Should_Remove_One_Item_From_Cart()
    { 
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var addItemToCartCommandHandler = new AddItemToCartCommandHandler(orderRepository, productRepository);

        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct1.Value, 4), CancellationToken.None);
        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct2.Value, 4), CancellationToken.None);
        await addItemToCartCommandHandler.Handle(new AddItemToCartCommand(outputCreateCustomer.Value, outputCreateProduct3.Value, 1), CancellationToken.None);

        var getCartQueryHandler = new GetCartQueryHandler(orderRepository);

        var outputGetCartBeforeRemove = await getCartQueryHandler.Handle(new GetCartQuery(), CancellationToken.None);

        var lineItemToRemove = outputGetCartBeforeRemove.Value.Items?.FirstOrDefault(li => li.ProductId == outputCreateProduct3.Value);

        var removeLineItemFromCartCommandHandler = new RemoveItemFromCartCommandHandler(orderRepository);

        await removeLineItemFromCartCommandHandler.Handle(new RemoveItemFromCartCommand(lineItemToRemove!.LineItemId), CancellationToken.None);

        var outputGetCartAfterRemove = await getCartQueryHandler.Handle(new GetCartQuery(), CancellationToken.None);

        Assert.Equal(2, outputGetCartAfterRemove.Value?.Items?.Count());
        Assert.Equal("cart", outputGetCartAfterRemove.Value?.Status);
        Assert.Equal(440, outputGetCartAfterRemove.Value?.Total);
    }

    [Fact]
    public async Task Should_Get_Orders_By_CustomerId()
    {
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var inputCreateOrder = new CheckoutOrderCommand(new List<OrderItemRequest>()
            {
                new OrderItemRequest(outputCreateProduct1.Value, 2),
                new OrderItemRequest(outputCreateProduct2.Value, 3),
                new OrderItemRequest(outputCreateProduct3.Value, 4)
            },
            outputCreateCustomer.Value,
            outputCreateAddress.Value,
            outputCreateAddress.Value,
            null,
            paymentType,
            cardToken,
            installments);

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository, productRepository, addressRepository, couponRepository, queue);

        //WHEN
        await checkoutOrderCommandHandler.Handle(inputCreateOrder, CancellationToken.None);
        await checkoutOrderCommandHandler.Handle(inputCreateOrder, CancellationToken.None);
        await checkoutOrderCommandHandler.Handle(inputCreateOrder, CancellationToken.None);

        var outputGetOrders = await orderRepository.GetOrdersByCustomerId(outputCreateCustomer.Value, CancellationToken.None);

        Assert.Equal(3, outputGetOrders.Count());
    }

    [Fact]
    public async Task Should_Cancel_Order()
    {
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var inputCreateOrder = new CheckoutOrderCommand(new List<OrderItemRequest>()
            {
                new OrderItemRequest(outputCreateProduct1.Value, 2),
                new OrderItemRequest(outputCreateProduct2.Value, 3),
                new OrderItemRequest(outputCreateProduct3.Value, 4)
            },
            outputCreateCustomer.Value,
            outputCreateAddress.Value,
            outputCreateAddress.Value,
            null,
            paymentType,
            cardToken,
            installments);

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository, productRepository, addressRepository, couponRepository, queue);

        //WHEN
        var outputCheckoutOrder = await checkoutOrderCommandHandler.Handle(inputCreateOrder, CancellationToken.None);

        var cancelOrderCommandHandler = new CancelOrderCommandHandler(orderRepository, queue);

        await cancelOrderCommandHandler.Handle(new CancelOrderCommand(outputCheckoutOrder.Value), CancellationToken.None);

        var outputGetOrder = await orderRepository.GetByIdAsync(outputCheckoutOrder.Value, CancellationToken.None);

        Assert.Equal("canceled", outputGetOrder?.Status);
    }

    [Fact]
    public async Task Should_Prepare_Order_For_Shipping()
    {
        var inputCreateCustomer = new CreateCustomerCommand("John Doe", "john.doe@gmail.com", "abc123", new DateTime(2004, 11, 06), "659.232.850-96", "(11) 97414-6507");

        var createCustomerCommandHandler = new CreateCustomerCommandHandler(customerRepository, passwordHasher, queue);

        var outputCreateCustomer = await createCustomerCommandHandler.Handle(inputCreateCustomer, CancellationToken.None);

        var inputCreateCategory = new CreateCategoryCommand("Category", "Category Description");

        var createCategoryCommandHandler = new CreateCatagoryCommandHandler(categoryRepository);

        var outputCreateCategory = await createCategoryCommandHandler.Handle(inputCreateCategory, CancellationToken.None);

        var inputCreateProduct1 = new CreateProductCommand("Product 1", "Product 1", "BRL", 50, "Image", "0001", outputCreateCategory.Value);
        var inputCreateProduct2 = new CreateProductCommand("Product 2", "Product 2", "BRL", 60, "Image", "0002", outputCreateCategory.Value);
        var inputCreateProduct3 = new CreateProductCommand("Product 3", "Product 3", "BRL", 70, "Image", "0003", outputCreateCategory.Value);

        var createProductCommandHandler = new CreateProductCommandHandler(productRepository, categoryRepository);

        var outputCreateProduct1 = await createProductCommandHandler.Handle(inputCreateProduct1, CancellationToken.None);
        var outputCreateProduct2 = await createProductCommandHandler.Handle(inputCreateProduct2, CancellationToken.None);
        var outputCreateProduct3 = await createProductCommandHandler.Handle(inputCreateProduct3, CancellationToken.None);

        var inputCreateAddress = new CreateAddressCommand(outputCreateCustomer.Value, "12909-062", "Rua a", "Bairro", "100", null, "São Paulo", "SP", "Brazil");

        var createAddressCommandHandler = new CreateAddressCommandHandler(addressRepository);

        var outputCreateAddress = await createAddressCommandHandler.Handle(inputCreateAddress, CancellationToken.None);

        var paymentType = "credit";
        var cardToken = "my-token-card";
        var installments = 5;

        var inputCheckout = new CheckoutOrderCommand(new List<OrderItemRequest>()
            {
                new OrderItemRequest(outputCreateProduct1.Value, 2),
                new OrderItemRequest(outputCreateProduct2.Value, 3),
                new OrderItemRequest(outputCreateProduct3.Value, 4)
            },
            outputCreateCustomer.Value,
            outputCreateAddress.Value,
            outputCreateAddress.Value,
            null,
            paymentType,
            cardToken,
            installments);

        //WHEN

        var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(orderRepository, customerRepository, productRepository, addressRepository, couponRepository, queue);

        var outputCheckoutOrder = await checkoutOrderCommandHandler.Handle(inputCheckout, CancellationToken.None);

        var inputOrderPaymentStatusChanged = new OrderPaymentStatusChangedCommand(outputCheckoutOrder.Value, "approved");

        var orderPaymentStatusChangedCommandHandler = new OrderPaymentStatusChangedCommandHandler(orderRepository);

        await orderPaymentStatusChangedCommandHandler.Handle(inputOrderPaymentStatusChanged, CancellationToken.None); 

        var inputPrepareOrderForShipping = new PrepareOrderForShippingCommand(outputCheckoutOrder.Value);

        var prepareOrderForShippingCommandHandler = new PrepareOrderForShippingCommandHandler(orderRepository);

       await prepareOrderForShippingCommandHandler.Handle(inputPrepareOrderForShipping, CancellationToken.None); 

        var outputGetOrder = await orderRepository.GetByIdAsync(outputCheckoutOrder.Value, CancellationToken.None);

        Assert.Equal("in_preparation", outputGetOrder?.Status);
    }
}
