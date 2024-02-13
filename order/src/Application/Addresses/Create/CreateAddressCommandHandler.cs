using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Addresses;
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
                request.Apartment,
                request.City,
                request.State,
                request.Country
            );

        if (address.IsFailure) return Result.Failure<Guid>(address.Error);

        _addressRepository.Add(address.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return address.Value.Id;
    }
}
