using NotifyWorker.Events;
using NotifyWorker.Gateway;
using NotifyWorker.Model;

namespace NotifyWorker.Queue;

public class QueueController : BackgroundService
{
    private readonly IQueue _queue;
    private readonly IMailerGateway _mailerGateway;
    private readonly IOrderGateway _orderGateway;

    public QueueController(IQueue queue, IMailerGateway mailerGateway, IOrderGateway orderGateway)
    {
        _queue = queue;
        _mailerGateway = mailerGateway;
        _orderGateway = orderGateway;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<CustomerCreatedEvent>("customerCreated.notification", "customer.created", async @event => {
                var mailData = new Maildata()
                {
                    EmailBody = Templates.Welcome(@event.Name),
                    EmailSubject = "Welcome",
                    EmailToEmail = @event.Email,
                    EmailToName = @event.Name,
                };

                await _mailerGateway.Send(mailData);
            });

            await _queue.SubscribeAsync<OrderCanceled>("orderCanceled.notification", "order.canceled", async @event => 
            {
                var customer = await _orderGateway.GetCustomerById(@event.CustomerId);
                
                var mailData = new Maildata()
                {
                    EmailToEmail = customer.Email,
                    EmailToName = customer.Name,
                    EmailSubject = "Order canceled",
                    EmailBody = Templates.OrderCanceled(customer.Name, @event.OrderId),
                };
                await _mailerGateway.Send(mailData); 
            });

            await _queue.SubscribeAsync<OrderCheckedout>("orderCheckedout.notification", "order.checkedout", async message =>
            {
                var customer = await _orderGateway.GetCustomerById(message.CustomerId);

                var items = new List<OrderChecedoutItem>();
                foreach (var item in message.Items) 
                { 
                    var product = await _orderGateway.GetProductById(item.ProductId);
                    items.Add(new OrderChecedoutItem(item.Id, product.Name, item.Quantity, item.Price));
                }

                var mailModel = new OrderCheckedoutMailModel(customer.Name, message.OrderId, DateTime.Now, items);

                var mailData = new Maildata()
                {
                    EmailToEmail = customer.Email,
                    EmailToName = customer.Name,
                    EmailSubject = "Order Created",
                    EmailBody = Templates.OrderCheckedout(mailModel)
                };

                await _mailerGateway.Send(mailData);
            });

            await _queue.SubscribeAsync<TransactionStatusChanged>("transactionApproved.notification", "transaction.status.changed", async message => 
            {
                var customer = await _orderGateway.GetCustomerByOrderId(message.OrderId);

                var order = await _orderGateway.GetOrderById(message.OrderId);
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

                await _mailerGateway.Send(mailData);
            });

            await _queue.SubscribeAsync<OrderShipped>("orderShipped.notification", "order.shipped", async @event => {
                var order = await _orderGateway.GetOrderById(@event.OrderId);

                var customer = await _orderGateway.GetCustomerById(order.CustomerId);

                var mailData = new Maildata() 
                {
                    EmailToName = customer.Name,
                    EmailToEmail = customer.Email,
                    EmailBody = Templates.OrderShipped(customer.Name, order.Id),
                    EmailSubject = "Order Shipped!"
                };

                await _mailerGateway.Send(mailData);
            });
        }
    }
}
