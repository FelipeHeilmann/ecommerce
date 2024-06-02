using NotifyWorker.Events;
using NotifyWorker.Gateway;
using NotifyWorker.Model;
using Application.Abstractions.Queue;

namespace NotifyWorker.Consumers;

public class OrderPaymentStatusChangedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IQueue _queue;
    public OrderPaymentStatusChangedConsumer(IServiceProvider serviceProvider, IQueue queue)
    {
        _serviceProvider = serviceProvider;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<TransactionStatusChanged>("transactionApproved.notification", "transaction.status.changed", async message => 
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var mailerGateway = scope.ServiceProvider.GetRequiredService<IMailerGateway>();

                var orderGateway = scope.ServiceProvider.GetRequiredService<IOrderGateway>();

                var customer = await orderGateway.GetCustomerByOrderId(message.OrderId);

                var order = await orderGateway.GetOrderById(message.OrderId);
                var items = new List<OrderChecedoutItem>();
                foreach (var item in order.Items)
                {                     
                    items.Add(new OrderChecedoutItem(item.Id, item.Name, item.Quantity, item.Price));
                }

                var mailModel = new OrderCheckedoutMailModel(customer.Name, message.OrderId, DateTime.Now, items);
                
                var mailData = new Maildata()
                {
                    EmailSubject = message.Status == "approved" ? "Payment Approved" : "Payment Reject",
                    EmailToEmail = customer.Email,
                    EmailToName = customer.Name,
                    EmailBody = message.Status == "approved" ? Templates.PaymentApproved(mailModel) : Templates.PaymentRejected(mailModel)
                };

                await mailerGateway.Send(mailData);
            }
        });
    }
}
