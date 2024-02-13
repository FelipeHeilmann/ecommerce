using Application.Abstractions.Messaging;
using Domain.Addresses;
using Domain.Shared;

namespace Application.Addresses.GetByCustomerId;

public class GetAddressesByCustomerIdQueryHandler : IQueryHandler<GetAddressesByCustomerIdQuery, ICollection<Address>>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressesByCustomerIdQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<ICollection<Address>>> Handle(GetAddressesByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return Result.Success(addresses);
    }
}
