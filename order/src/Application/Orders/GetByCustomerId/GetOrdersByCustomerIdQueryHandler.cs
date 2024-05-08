using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetByCustomerId;

public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerQuery, ICollection<Output>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersByCustomerIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetOrdersByCustomerId(query.CustomerId, cancellationToken, "Items");

        var output = new List<Output>();

        foreach (var order in orders) 
        {
            output.Add(new Output(
                order.Id,
                order.CustomerId,
                order.Status.ToString(),
                order.Items.Select(line => new ItemsOutput(line.ProductId, line.Price.Amount, line.Quantity)),
                order.BillingAddressId,
                order.ShippingAddressId)
            );
        }

        return output;
    }
}
