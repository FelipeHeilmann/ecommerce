using Application.Abstractions.Query;
using Application.Abstractions.Queue;
using Domain.Customers.Repository;
using Domain.Orders.Events;
using Domain.Orders.Repository;
using Domain.Products.Repository;
using MediatR;

namespace Application.Orders.EventsHandler;

public class SendOrderCheckoutedMail : INotificationHandler<OrderCheckedout>
{
    private readonly IQueue _queue;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
   

    public SendOrderCheckoutedMail(IQueue queue, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository)
    {
        _queue = queue;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task Handle(OrderCheckedout notification, CancellationToken cancellationToken)
    {
        var order = (OrderCheckedoutData)notification.Data;
   
        var customer = await _customerRepository.GetByIdAsync(order!.CustomerId, cancellationToken);

        List<OrderCheckedoutIMailItem> products = new List<OrderCheckedoutIMailItem>();

        foreach (var orderItem in order.Items) 
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);
            products.Add(new OrderCheckedoutIMailItem(product!.Name, product!.Price.Amount, orderItem.Quantity));
        }

        await _queue.PublishAsync(new OrderCheckedoutMail(order.OrderId, DateTime.Now, customer!.Name, customer.Email, products), "order.checkedout");
    }
}
