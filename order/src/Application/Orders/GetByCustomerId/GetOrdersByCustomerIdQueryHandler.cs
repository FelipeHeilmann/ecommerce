﻿using Application.Abstractions.Messaging;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Orders.GetByCustomerId;

public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerIdQuery, ICollection<Output>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersByCustomerIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetOrdersByCustomerId(query.CustomerId, cancellationToken);

        var output = new List<Output>();

        foreach (var order in orders) 
        {
            output.Add(new Output(
                order.Id,
                order.CustomerId,
                order.GetStatus(),
                order.Items.Select(line => new ItemsOutput(line.ProductId, line.Price.Amount, line.Quantity)),
                order.BillingAddressId,
                order.ShippingAddressId)
            );
        }

        return output;
    }
}
