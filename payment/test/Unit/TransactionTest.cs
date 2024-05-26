using Domain.Transactions.Entity;
using Domain.Transactions.VO;
using Xunit;
namespace Unit;

public class TransactionTest
{
    [Fact]
    public void Should_Create_Credit_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "credit";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount ,paymentType);

        Assert.Equal("credit_card", transaction.PaymentType);
        Assert.Equal("waiting_payment", transaction.Status);
    }

    [Fact]
    public void Should_Create_Debit_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "debit";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        Assert.Equal("debit_card", transaction.PaymentType);
        Assert.Equal("waiting_payment", transaction.Status);
    }

    [Fact]
    public void Should_Create_Pix_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "pix";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        Assert.Equal("pix", transaction.PaymentType);
        Assert.Equal("waiting_payment", transaction.Status);
    }

    [Fact]
    public void Should_Create_Billet_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "billet";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        Assert.Equal("billet", transaction.PaymentType);
        Assert.Equal("waiting_payment", transaction.Status);
    }

    [Fact]
    public void Should_Approve_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "billet";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        transaction.Approve();

        Assert.Equal("approved", transaction.Status);
    }

    [Fact]
    public void Should_Refused_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "billet";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        transaction.Reject();

        Assert.Equal("rejected", transaction.Status);
    }

    [Fact]
    public void Should_Refund_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var amount = 300.00;
        var paymentType = "billet";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, amount, paymentType);

        transaction.Refund();

        Assert.Equal("refunding", transaction.Status);
    }
}
