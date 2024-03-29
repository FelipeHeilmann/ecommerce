﻿using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.GetByCustomerId;

public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerQuery, ICollection<Order>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersByCustomerIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Order>>> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        var products = await _repository.GetOrdersByCustomerId(query.CustomerId, cancellationToken, "Items");

        return Result.Success(products);
    }
}
