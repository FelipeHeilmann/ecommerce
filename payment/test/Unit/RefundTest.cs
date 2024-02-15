using Xunit;
using Domain.Refunds;
namespace Unit;

public class RefundTest
{
    [Fact]
    public void Should_Create_Refund()
    {
        var trasactionId = Guid.NewGuid();
        var amount = 300.0;
        var refund = Refund.Create(trasactionId, amount);

        Assert.Equal(RefundStatus.WaitingRefund, refund.Status);
    }

    [Fact]
    public void Should_Pay_Refund()
    {
        var trasactionId = Guid.NewGuid();
        var amount = 300.0;
        var refund = Refund.Create(trasactionId, amount);

        refund.Pay();

        Assert.Equal(RefundStatus.RefundPayed, refund.Status);
    }
}
