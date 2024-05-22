using Application.Abstractions.Messaging;
using Application.Abstractions.Query;
using Domain.Shared;

namespace Application.Orders.GetByCustomerId;

public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerIdQuery, ICollection<OrderQueryModel>>
{
    private readonly IOrderQueryContext _context;

    public GetOrdersByCustomerIdQueryHandler(IOrderQueryContext context)
    {
        _context = context;
    }

    public async Task<Result<ICollection<OrderQueryModel>>> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var orders = await _context.GetByCustomerId(query.CustomerId);
        return orders.ToList();
    }
}
