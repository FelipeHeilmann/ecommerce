using Domain.DomainEvents;
using Xunit;

namespace Integration;

public class TransactionTest
{
    //private readonly ITransactionRepository _transactionRepository new TransactionRepostoryMemory();

    //[Fact]
    //public async void Should_Recive_Order_Payment_Make_Request_To_Payment_Gateway_Create_Transaction()
    //{
    //    var lineItems = new List<LineItemOrderCompletedEvent>()
    //    {
    //        new LineItemOrderCompletedEvent(Guid.NewGuid(), Guid.NewGuid(),2, 30.0),
    //        new LineItemOrderCompletedEvent(Guid.NewGuid(), Guid.NewGuid(),3, 50.0),
    //        new LineItemOrderCompletedEvent(Guid.NewGuid(), Guid.NewGuid(),4, 20.0),
    //        new LineItemOrderCompletedEvent(Guid.NewGuid(), Guid.NewGuid(),6, 80.0),
    //    };

    //    var orderPaymnetRequest = new OrderCompletedEvent(
    //        Guid.NewGuid(),
    //        50.00,
    //        lineItems,
    //        "Felipe Heilmann",
    //        "felipeheilmannm@gmail.com",
    //        "credit",
    //        "my-token-card",
    //        2,
    //        "04182-135",
    //        "112",
    //        null,
    //        "04182-135",
    //        "112",
    //        null
    //        );

    //    var consumer = new OrderCompletedConsumer(_transactionRepository);

    //    await consumer.Consume(orderPaymnetRequest);

    //}
}
