using Application.Abstractions.Messaging;
using Domain.Customers.Error;
using Domain.Customers.Repository;
using Domain.Shared;

namespace Application.Customers.GetById;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, Output>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<Output>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(query.customerId, cancellationToken);

        if (customer == null) return Result.Failure<Output>(CustomerErrors.CustomerNotFound);

        return new Output(
                        customer.Id,
                        customer.GetName(),
                        customer.Email.Value,
                        customer.GetCPF(),
                        customer.GetPhone(),
                        customer.BirthDate
                    );  
    }
}
