using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectionWorker.Context;
using ProjectionWorker.Gateway;
using ProjectionWorker.Model;

namespace ProjectionWorker.Queue;

public class QueueController : BackgroundService
{
    private readonly IQueue _queue;
    private readonly OrderContext _orderContext;
    private readonly IOrderGeteway _orderGateway;

    public QueueController(IQueue queue, OrderContext orderContext, IOrderGeteway orderGeteway)
    {
        _queue = queue;
        _orderContext = orderContext;
        _orderGateway = orderGeteway;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await _queue.SubscribeAsync<Events.OrderCheckedout>("orderCheckedout.updateProggresion", "order.checkedout", async @event => 
            {
                var items = new List<ItemModel>();

                foreach(var item in @event.Items)
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

                var address = await _orderGateway.GetAddressById(@event.AddressId);

                var orderModel = new OrderModel() 
                {
                    CustomerId = @event.CustomerId.ToString(),
                    OrderId = @event.OrderId.ToString(),
                    Status = "waiting_payment",
                    Address = new AddressModel() 
                    { 
                        Id = @event.AddressId.ToString(),
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
                        Installments = @event.Installment,
                        PaymentType = @event.PaymentType,
                    },
                };
                await _orderContext.Save(orderModel);
            });

            await _queue.SubscribeAsync<Events.TransactionStatusChanged>("transactionStatusChanged.updateProjection", "transaction.status.changed", async @event => 
            {
                var order = await _orderContext.GetById(@event.OrderId);

                if(@event.Status == "approved") 
                {
                    order.Status = "payment_approved";
                    order.Payment!.PayedAt = @event.ApprovedOrRejectedAt;
                }
                else
                {
                    order.Status = "payment_rejected";
                    order.Payment!.PayedAt = @event.ApprovedOrRejectedAt;
                }

                await _orderContext.Update(order);
            });
            
            await _queue.SubscribeAsync<Events.OrderCanceled>("orderCanceled.updateProjection", "order.canceled", async @event => 
            {
                var order = await _orderContext.GetById(@event.OrderId);

                order.Status = "canceled";

                await _orderContext.Update(order);
            });  
        }
    }
}
