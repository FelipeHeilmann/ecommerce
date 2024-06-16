using Application.Abstractions.Gateway;
using Application.Abstractions.Queue;
using Application.Transactions.MakePaymentRequest;
using Application.Transactions.ProccessTransaction;
using Domain.Events;
using Domain.Transactions.Repository;
using Infra.Gateway.Payment;
using Infra.Queue;
using Infra.Repositories.Memory;
using Moq;
using Xunit;

namespace Integration;

public class TransactionTest
{
    private readonly ITransactionRepository transactionRepository =  new TransactionRepositoryMemory();
    private readonly IPaymentGateway paymentGateway = new PaymentGatewayFake();
    private readonly IQueue queue = new MemoryQueueAdapter();

    [Fact]
    public async void Should_Create_Transaction()
    {
        var orderGateway = new Mock<IOrderGateway>();

        orderGateway.Setup(m => m.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync(new CustomerGatewayResponse(
             Id: Guid.NewGuid(),
             Name: "John Doe",
             Email: "john.doe@example.com",
             CPF: "123.456.789-00",
             Phone: "+1234567890",
             BirthDate: new DateOnly(1990, 5, 15),
             CreatedAt: DateTime.UtcNow
         ));

        orderGateway.Setup(m => m.GetAddressById(It.IsAny<Guid>())).ReturnsAsync(new AddressGatewayResponse(
            Id: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            ZipCode: "12345-678",
            Street: "123 Main St",
            Neighborhood: "Downtown",
            Number: "456",
            Complement: "Apt 101",
            City: "Cityville",
            State: "ST",
            Country: "Countryland"
        ));


        var items = new List<LineItemOrderCheckedout>()
        {
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),2, 30.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),3, 50.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),4, 20.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),6, 80.0),
        };

        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();  
        var addressId = Guid.NewGuid();

        var inputCreateTransaction = new CreatePaymentCommand(new OrderCheckedout(orderId, items.Sum(i => i.Quantity * i.Price), items, customerId, "credit", "my-token", 5, addressId));

        var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, queue, orderGateway.Object);

        var output = await commandHandler.Handle(inputCreateTransaction, CancellationToken.None);

        Assert.True(output.IsSuccess);
        Assert.False(output.IsFailure);

    }

    [Fact]
    public async void Should_Approve_Transcation()
    {
        var orderGateway = new Mock<IOrderGateway>();

        orderGateway.Setup(m => m.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync(new CustomerGatewayResponse(
             Id: Guid.NewGuid(),
             Name: "John Doe",
             Email: "john.doe@example.com",
             CPF: "123.456.789-00",
             Phone: "+1234567890",
             BirthDate: new DateOnly(1990, 5, 15),
             CreatedAt: DateTime.UtcNow
         ));

        orderGateway.Setup(m => m.GetAddressById(It.IsAny<Guid>())).ReturnsAsync(new AddressGatewayResponse(
            Id: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            ZipCode: "12345-678",
            Street: "123 Main St",
            Neighborhood: "Downtown",
            Number: "456",
            Complement: "Apt 101",
            City: "Cityville",
            State: "ST",
            Country: "Countryland"
        ));


        var items = new List<LineItemOrderCheckedout>()
        {
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),2, 30.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),3, 50.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),4, 20.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),6, 80.0),
        };

        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        var inputCreateTransaction = new CreatePaymentCommand(new OrderCheckedout(orderId, items.Sum(i => i.Quantity * i.Price), items, customerId, "credit", "my-token", 5, addressId));

        var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, queue, orderGateway.Object);

        var outputCreateTransaction = await commandHandler.Handle(inputCreateTransaction, CancellationToken.None);

        var inputProccessTransaction = new ProccessTransactionCommand(outputCreateTransaction.Value, "approved");

        var proccessTransactionCommandHandler = new ProccessTransactionCommandHandler(transactionRepository, queue);

        await proccessTransactionCommandHandler.Handle(inputProccessTransaction, CancellationToken.None);

        var outputGetTransaction = await transactionRepository.GetByIdAsync(outputCreateTransaction.Value, CancellationToken.None);

        Assert.Equal("approved", outputGetTransaction?.Status);
    }

    [Fact]
    public async void Should_Reject_Transcation()
    {
        var orderGateway = new Mock<IOrderGateway>();

        orderGateway.Setup(m => m.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync(new CustomerGatewayResponse(
             Id: Guid.NewGuid(),
             Name: "John Doe",
             Email: "john.doe@example.com",
             CPF: "123.456.789-00",
             Phone: "+1234567890",
             BirthDate: new DateOnly(1990, 5, 15),
             CreatedAt: DateTime.UtcNow
         ));

        orderGateway.Setup(m => m.GetAddressById(It.IsAny<Guid>())).ReturnsAsync(new AddressGatewayResponse(
            Id: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            ZipCode: "12345-678",
            Street: "123 Main St",
            Neighborhood: "Downtown",
            Number: "456",
            Complement: "Apt 101",
            City: "Cityville",
            State: "ST",
            Country: "Countryland"
        ));


        var items = new List<LineItemOrderCheckedout>()
        {
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),2, 30.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),3, 50.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),4, 20.0),
            new LineItemOrderCheckedout(Guid.NewGuid(), Guid.NewGuid(),6, 80.0),
        };

        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var addressId = Guid.NewGuid();

        var inputCreateTransaction = new CreatePaymentCommand(new OrderCheckedout(orderId, items.Sum(i => i.Quantity * i.Price), items, customerId, "credit", "my-token", 5, addressId));

        var commandHandler = new CreatePaymentCommandHandler(paymentGateway, transactionRepository, queue, orderGateway.Object);

        var outputCreateTransaction = await commandHandler.Handle(inputCreateTransaction, CancellationToken.None);

        var inputProccessTransaction = new ProccessTransactionCommand(outputCreateTransaction.Value, "rejected");

        var proccessTransactionCommandHandler = new ProccessTransactionCommandHandler(transactionRepository, queue);

        await proccessTransactionCommandHandler.Handle(inputProccessTransaction, CancellationToken.None);

        var outputGetTransaction = await transactionRepository.GetByIdAsync(outputCreateTransaction.Value, CancellationToken.None);

        Assert.Equal("rejected", outputGetTransaction?.Status);
    }
}
