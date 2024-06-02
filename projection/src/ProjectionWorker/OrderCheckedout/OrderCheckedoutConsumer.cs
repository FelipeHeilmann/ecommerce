using ProjectionWorker.Context;
using ProjectionWorker.Gateway;
using ProjectionWorker.Model;
using ProjectionWorker.Queue;

namespace ProjectionWorker.OrderCheckedout;

public class OrderCheckedoutConsumer : BackgroundService
{
    private IQueue _queue;
    private readonly OrderContext _orderQueryContext;
    private readonly IOrderGeteway _orderGateway;

    public OrderCheckedoutConsumer(IQueue queue, OrderContext orderQueryContext, IOrderGeteway orderGateway)
    {
        _queue = queue;
        _orderQueryContext = orderQueryContext;
        _orderGateway = orderGateway;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queue.SubscribeAsync<Events.OrderCheckedout>("orderCheckedout.updateProggresion", "order.checkedout", async message => 
        {
            var items = new List<ItemModel>();

            foreach(var item in message.Items)
            {
                var product = await _orderGateway.GetProductById(item.ProductId);
                items.Add(new ItemModel()
                {
                    Id = item.Id.ToString(),
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    Price = product.Amount,
                    ProductId = product.Id.ToString(),
                    Quantity = item.Quantity
                });
            }

            var address = await _orderGateway.GetAddressById(message.AddressId);

            var orderModel = new OrderModel() 
            {
                CustomerId = message.CustomerId.ToString(),
                OrderId = message.OrderId.ToString(),
                Status = "waiting_payment",
                Address = new AddressModel() 
                { 
                    Id = message.AddressId.ToString(),
                    Neighborhood = address.Neighborhood,
                    City = address.City,
                    Complement = address.Complement,
                    Country = address.Country,
                    Number = address.Number,
                    State = address.State,
                    Street = address.Street,
                    ZipCode = address.ZipCode,
                },
                Items = items,
                Payment = new PaymentModel()
                {
                    PayedAt = null,
                    Installments = message.Installment,
                    PaymentType = message.PaymentType,
                },
            };
            await _orderQueryContext.Save(orderModel);
        });
    }
}
