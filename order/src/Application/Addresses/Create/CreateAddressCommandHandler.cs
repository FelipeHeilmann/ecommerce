using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Addresses.Entity;
using Domain.Addresses.Repository;
using Domain.Shared;

namespace Application.Addresses.Create;

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, Guid>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var address = Address.Create
            (
                request.CustomerId,
                request.Zipcode,
                request.Street,
                request.Neighborhood,
                request.Number,
                request.Complement,
                request.City,
                request.State,
                request.Country
            );

        _addressRepository.Add(address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return address.Id;
    }
}
