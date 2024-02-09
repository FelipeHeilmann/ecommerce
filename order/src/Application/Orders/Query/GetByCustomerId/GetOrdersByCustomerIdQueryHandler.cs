using Application.Abstractions;
using Domain.Orders;
using Domain.Shared;

namespace Application.Orders.Query.GetByCustomerId;

public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerQuery, ICollection<Order>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersByCustomerIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ICollection<Order>>> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        var products = await _repository.GetOrdersByCustomerId(query.CustomerId, cancellationToken);

        return Result.Success(products);
    }
}
