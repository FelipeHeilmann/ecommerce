using Application.Abstractions.Messaging;
using Domain.Addresses;
using Domain.Shared;

namespace Application.Addresses.GetById;

public class GetAddressByIdQueryHandler : IQueryHandler<GetAddressByIdQuery, Address>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<Address>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(query.AddressId, cancellationToken);

        if (address == null) return Result.Failure<Address>(AddressErrors.NotFound);

        return address;
    }
}
