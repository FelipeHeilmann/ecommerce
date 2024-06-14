using Application.Abstractions.Messaging;
using Domain.Addresses.Entity;
using Domain.Addresses.Repository;
using Domain.Shared;

namespace Application.Addresses.Create;

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, Guid>
{
    private readonly IAddressRepository _addressRepository;

    public CreateAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Result<Guid>> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
    {

        var address = Address.Create
            (
                command.CustomerId,
                command.Zipcode,
                command.Street,
                command.Neighborhood,
                command.Number,
                command.Complement,
                command.City,
                command.State,
                command.Country
            );

        await _addressRepository.Add(address);
        
        return address.Id;
    }
}
