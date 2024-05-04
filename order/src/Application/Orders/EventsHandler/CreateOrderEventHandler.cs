using Application.Abstractions.Queue;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using MediatR;

namespace Application.Orders.EventsHandler;

public class CreateOrderEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IQueue _queue;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderEventHandler(IQueue queue, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository)
    {
        _queue = queue;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken, "");

        var custmer = await _customerRepository.GetByIdAsync(order!.CustomerId, cancellationToken);

        List<OrderCreatedItem> products = new List<OrderCreatedItem>();

        foreach (var orderItem in order.Items) 
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);
            products.Add(new OrderCreatedItem(product!.Name, product!.Price.Amount, orderItem.Quantity));
        }

        await _queue.PublishAsync(new OrderCreatedMailEvent(order.Id, DateTime.Now, custmer!.Name, custmer.Email, products), "order.create");
    }
}
