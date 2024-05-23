using Application.Abstractions.Query;
using Application.Abstractions.Queue;
using Domain.Customers.Repository;
using Domain.Orders.Events;
using Domain.Orders.Repository;
using Domain.Products.Repository;
using MediatR;

namespace Application.Orders.EventsHandler;

public class OrderCheckedoutEventHandler : INotificationHandler<OrderCheckedout>
{
    private readonly IQueue _queue;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderQueryContext _orderQueryContext;

    public OrderCheckedoutEventHandler(IQueue queue, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IOrderQueryContext orderQueryContext)
    {
        _queue = queue;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _orderQueryContext = orderQueryContext;
    }

    public async Task Handle(OrderCheckedout notification, CancellationToken cancellationToken)
    {
        var order = (OrderCheckedoutData)notification.Data;
   
        var customer = await _customerRepository.GetByIdAsync(order!.CustomerId, cancellationToken);

        var orderQueryModel = new OrderQueryModel()
        {
            Id = order.OrderId,
            CustomerId = order.CustomerId,
            PayedAt = null,
            Status = "waiting_payment",
            Items = new List<LineItemQueryModel>()

        };

        List<OrderCheckedoutIMailItem> products = new List<OrderCheckedoutIMailItem>();

        foreach (var orderItem in order.Items) 
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);
            products.Add(new OrderCheckedoutIMailItem(product!.Name, product!.Price.Amount, orderItem.Quantity));
            orderQueryModel.Items.Add(new LineItemQueryModel()
            {
                Id = orderItem.Id,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = orderItem.Price,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
            });
        }

        await _orderQueryContext.Save(orderQueryModel);

        await _queue.PublishAsync(new OrderCheckedoutMail(order.OrderId, DateTime.Now, customer!.Name, customer.Email, products), "order.checkedout");
    }
}
