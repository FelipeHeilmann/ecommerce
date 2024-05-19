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

        var address = await _addressRepository.GetByIdAsync(command.Id, cancellationToken);

        if (address == null) return Result.Failure<Address>(AddressErrors.NotFound);

        address.Update(
            command.Zipcode,
            command.Street,
            command.Neighborhood,
            command.Number,
            command.Complement,
            command.City,
            command.State,
            command.Country
         );

        _addressRepository.Update(address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
