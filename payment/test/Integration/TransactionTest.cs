using Application.Abstractions.Gateway;
using Application.Data;
using Application.Transactions.MakePaymentRequest;
using Domain.Events;
using Domain.Transactions;
using Infra.Data;
using Infra.Gateway.Payment;
using Infra.Repositories.Memory;
using Xunit;

namespace Integration;

public class TransactionTest
{
    private readonly ITransactionRepository _transactionRepository =  new TransactionRepositoryMemory();
    private readonly IPaymentGateway _paymentGateway = new PaymentGatewayMemory();
    private readonly IUnitOfWork _unitOfWord = new UnitOfWorkMemory();

    [Fact]
    public async void Should_Recive_Order_Payment_Make_Request_To_Payment_Gateway_Create_Transaction()
    {
        var lineitems = new List<LineItemOrderPurchasedEvent>()
        {
            new LineItemOrderPurchasedEvent(Guid.NewGuid(), Guid.NewGuid(),2, 30.0),
            new LineItemOrderPurchasedEvent(Guid.NewGuid(), Guid.NewGuid(),3, 50.0),
            new LineItemOrderPurchasedEvent(Guid.NewGuid(), Guid.NewGuid(),4, 20.0),
            new LineItemOrderPurchasedEvent(Guid.NewGuid(), Guid.NewGuid(),6, 80.0),
        };

        var orderPurchasedEvent = new OrderPurchasedEvent(
            Guid.NewGuid(),
            50.00,
            lineitems,
            Guid.NewGuid(),
            "felipe heilmann",
            "felipeheilmannm@gmail.com",
            "44444444444",
            "11 97414-9507",
            "credit",
            "my-token-card",
            2,
            "04182-135",
            "112",
            null
            );

        var command = new CreatePaymentCommand(orderPurchasedEvent);

        var commandHandler = new CreatePaymentCommandHandler(_paymentGateway, _transactionRepository, _unitOfWord);

        var result = await commandHandler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

    }
}
