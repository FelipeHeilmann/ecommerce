using ProjectionWorker.Context;
using ProjectionWorker.Queue;

namespace ProjectionWorker.OrderCanceled;

public class OrderCanceledConsumer : BackgroundService
{
    private readonly IQueue _queue;
    private readonly OrderContext _context;

    public OrderCanceledConsumer(IQueue queue, OrderContext orderContext)
    {
        _queue = queue;
        _context = orderContext;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<Events.OrderCanceled>("orderCanceled.updateProjection", "order.canceled", async message => 
        {
            var order = await _context.GetById(message.OrderId);

            order.Status = "canceled";

            await _context.Update(order);
        });
    }
}
