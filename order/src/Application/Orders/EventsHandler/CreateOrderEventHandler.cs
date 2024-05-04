using Application.Gateway;
using Application.Orders.Model;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using Domain.Shared;
using MediatR;

namespace Application.Orders.EventsHandler;

public class CreateOrderEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly INotifyGateway _notifyGateway;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderEventHandler(INotifyGateway notifyGateway, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository)
    {
        _notifyGateway = notifyGateway;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken, "");

        var custmer = await _customerRepository.GetByIdAsync(order!.CustomerId, cancellationToken);

        List<ItemsMail> products = new List<ItemsMail>();

        foreach (var orderItem in order.Items) 
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);
            products.Add(new ItemsMail(product!.Name, product!.Price.Amount, orderItem.Quantity));
        }

        await _notifyGateway.SendOrderCreatedMail(new OrderCreatedMail(order.Id, DateTime.Now, custmer!.Name, custmer.Email, products));
    }
}
