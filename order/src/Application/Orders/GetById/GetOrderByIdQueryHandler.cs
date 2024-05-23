using Application.Abstractions.Messaging;
using Application.Abstractions.Query;
using Domain.Orders.Error;
using Domain.Shared;

namespace Application.Orders.GetById;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderQueryModel>
{
    private readonly IOrderQueryContext _context;

    public GetOrderByIdQueryHandler(IOrderQueryContext context)
    {
        _context = context;
    }

    public async Task<Result<OrderQueryModel>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _context.GetById(query.OrderId);

        if (order == null) return Result.Failure<OrderQueryModel>(OrderErrors.OrderNotFound);

        return order;
    }
}
