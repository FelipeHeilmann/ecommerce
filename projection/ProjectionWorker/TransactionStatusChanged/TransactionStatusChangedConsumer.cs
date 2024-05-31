
using ProjectionWorker.Context;
using ProjectionWorker.Queue;

namespace ProjectionWorker.TransactionStatusChanged;

public class TransactionStatusChangedConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly OrderContext _context;

    public TransactionStatusChangedConsumer(OrderContext context, IQueue queue)
    {
        _context = context;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<Events.TransactionStatusChanged>("transactionStatusChanged.updateProjection", "transaction.status.changed", async message => 
        {
            var order = await _context.GetById(message.OrderId);

            if(message.Status == "approved") 
            {
                order.Status = "payment_approved";
                order.Payment.PayedAt = message.ApprovedOrRejectedAt;
            }
            else
            {
                order.Status = "payment_rejected";
            }

            await _context.Update(order);
        });
    }
}
