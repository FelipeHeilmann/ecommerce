using Application.Abstractions.Messaging;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Orders.Error;
using Domain.Orders.Repository;
using Domain.Shared;

namespace Application.Customers.GetByOrderId;

public class GetCustomerByOrderIdQueryHandler : IQueryHandler<GetCustomerByOrderIdQuery, Output>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByOrderIdQueryHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result<Output>> Handle(GetCustomerByOrderIdQuery command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null) return Result.Failure<Output>(OrderErrors.OrderNotFound);

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId, cancellationToken);

        if (customer == null) return Result.Failure<Output>(CustomerErrors.CustomerNotFound);

        return new Output(
                       customer.Id,
                       customer.Name,
                       customer.Email,
                       customer.CPF,
                       customer.Phone,
                       customer.BirthDate
                   );
    }
}
