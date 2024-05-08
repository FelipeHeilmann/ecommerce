using Application.Abstractions.Messaging;
using Domain.Addresses;
using Domain.Shared;

namespace Application.Addresses.GetById;

public class GetAddressByIdQueryHandler : IQueryHandler<GetAddressByIdQuery, Output>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressByIdQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<Output>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(query.AddressId, cancellationToken);

        if (address == null) return Result.Failure<Output>(AddressErrors.NotFound);

        return new Output(
                        address.Id,
                        address.CustomerId,
                        address.ZipCode,
                        address.Street,
                        address.Neighborhood,
                        address.Number,
                        address.Complement,
                        address.City,
                        address.State,
                        address.Country
                    );
    }
}
