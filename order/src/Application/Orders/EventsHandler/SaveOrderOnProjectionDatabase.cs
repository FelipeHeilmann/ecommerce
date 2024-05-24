using Application.Abstractions.Query;
using Domain.Addresses.Repository;
using Domain.Orders.Events;
using Domain.Products.Repository;
using MediatR;

namespace Application.Orders.EventsHandler;

public class SaveOrderOnProjectionDatabase : INotificationHandler<OrderCheckedout>
{
    private readonly IOrderQueryContext _orderQueryContext;
    private readonly IProductRepository _productRepository;
    private readonly IAddressRepository _addressRepository;

    public SaveOrderOnProjectionDatabase(IOrderQueryContext orderQueryContext, IProductRepository productRepository, IAddressRepository addressRepository)
    {
        _orderQueryContext = orderQueryContext;
        _productRepository = productRepository;
        _addressRepository = addressRepository;
    }

    public async Task Handle(OrderCheckedout notification, CancellationToken cancellationToken)
    {
        var order = (OrderCheckedoutData)notification.Data;

        var address = await _addressRepository.GetByIdAsync(order.AddressId ,cancellationToken);

        var orderQueryModel = new OrderQueryModel()
        {
            Id = order.OrderId,
            CustomerId = order.CustomerId,
            PayedAt = null,
            Status = "waiting_payment",
            Items = new List<LineItemQueryModel>(),
            Address = new AddressQueryModel()
            {
                Id = address!.Id,
                City = address!.City,
                Complement = address!.Complement,
                Country = address!.Country,
                Neighborhood = address!.Neighborhood,
                Number = address!.Number,   
                State = address!.State,
                Street = address!.Street,
                ZipCode = address!.ZipCode
            }
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
