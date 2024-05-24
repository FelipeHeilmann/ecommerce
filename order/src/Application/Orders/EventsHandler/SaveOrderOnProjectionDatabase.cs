using Application.Abstractions.Query;
using Domain.Orders.Events;
using Domain.Products.Repository;
using MediatR;

namespace Application.Orders.EventsHandler;

public class SaveOrderOnProjectionDatabase : INotificationHandler<OrderCheckedout>
{
    private readonly IOrderQueryContext _orderQueryContext;
    private readonly IProductRepository _productRepository;

    public SaveOrderOnProjectionDatabase(IOrderQueryContext orderQueryContext, IProductRepository productRepository)
    {
        _orderQueryContext = orderQueryContext;
        _productRepository = productRepository;
    }

    public async Task Handle(OrderCheckedout notification, CancellationToken cancellationToken)
    {
        var order = (OrderCheckedoutData)notification.Data;

        var orderQueryModel = new OrderQueryModel()
        {
            Id = order.OrderId,
            CustomerId = order.CustomerId,
            PayedAt = null,
            Status = "waiting_payment",
            Items = new List<LineItemQueryModel>()

        };

        foreach (var orderItem in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);

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
    }
}
