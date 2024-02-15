
using Domain.Transactions;
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
        var paymentType = "credit";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId, paymentType);

        Assert.Equal(PaymentType.CreditCard, transaction.PaymentType);
        Assert.Equal(TransactionStatus.WaitingPayment, transaction.Status);
    }

    [Fact]
    public void Should_Create_Debit_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "debit";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId ,paymentType);

        Assert.Equal(PaymentType.DebitCard, transaction.PaymentType);
        Assert.Equal(TransactionStatus.WaitingPayment, transaction.Status);
    }

    [Fact]
    public void Should_Create_Pix_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "pix";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId ,paymentType);

        Assert.Equal(PaymentType.Pix, transaction.PaymentType);
        Assert.Equal(TransactionStatus.WaitingPayment, transaction.Status);
    }

    [Fact]
    public void Should_Create_Billet_Transaction()
    {
        var oderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "billet";

        var transaction = Transaction.Create(oderId, customerId, paymentServiceTransactionId,paymentType);

        Assert.Equal(PaymentType.Billet, transaction.PaymentType);
        Assert.Equal(TransactionStatus.WaitingPayment, transaction.Status);
    }

    [Fact]
    public void Should_Approve_Transaction()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "billet";

        var transaction = Transaction.Create(orderId, customerId, paymentServiceTransactionId ,paymentType);

        transaction.Approve();

        Assert.Equal(TransactionStatus.Approved, transaction.Status);
    }

    [Fact]
    public void Should_Refused_Transaction()
    {
        var oderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "billet";

        var transaction = Transaction.Create(oderId, customerId, paymentServiceTransactionId,paymentType);

        transaction.Refuse();

        Assert.Equal(TransactionStatus.Refused, transaction.Status);
    }

    [Fact]
    public void Should_Refund_Transaction()
    {
        var oderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var paymentServiceTransactionId = Guid.NewGuid();
        var paymentType = "billet";

        var transaction = Transaction.Create(oderId, customerId, paymentServiceTransactionId, paymentType);

        transaction.Refund();

        Assert.Equal(TransactionStatus.AwaitingRefund, transaction.Status);
    }
}
