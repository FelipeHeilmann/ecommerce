using Application.Abstractions.Messaging;
using Domain.Addresses;
using Domain.Shared;

namespace Application.Addresses.GetByCustomerId;

public class GetAddressesByCustomerIdQueryHandler : IQueryHandler<GetAddressesByCustomerIdQuery, ICollection<Output>>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressesByCustomerIdQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<ICollection<Output>>> Handle(GetAddressesByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        var output = new List<Output>();

        foreach (var address in addresses)
        {
            output.Add(new Output(
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
                    ));
        }
        
        return output;
    }
}
