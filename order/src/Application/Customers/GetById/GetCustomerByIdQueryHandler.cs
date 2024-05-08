using Application.Abstractions.Messaging;
using Domain.Customers;
using Domain.Shared;

namespace Application.Customers.GetById;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, Customer>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<Customer>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(query.customerId, cancellationToken);

        if (customer == null) return Result.Failure<Customer>(CustomerErrors.CustomerNotFound);

        return Result.Success(customer);  
    }
}
