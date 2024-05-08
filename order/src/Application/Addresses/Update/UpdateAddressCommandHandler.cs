using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Addresses.Entity;
using Domain.Addresses.Error;
using Domain.Addresses.Repository;
using Domain.Shared;

namespace Application.Addresses.Update;

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork unitOfWork)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var request = command.request;

        var address = await _addressRepository.GetByIdAsync(request.Id, cancellationToken);

        if (address == null) return Result.Failure<Address>(AddressErrors.NotFound);

        var updatedAddress = address.Update(
            request.Zipcode,
            request.Street,
            request.Neighborhood,
            request.Number,
            request.Complement,
            request.City,
            request.State,
            request.Country
         );

        if (updatedAddress.IsFailure) return Result.Failure(updatedAddress.Error);

        _addressRepository.Update(address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
