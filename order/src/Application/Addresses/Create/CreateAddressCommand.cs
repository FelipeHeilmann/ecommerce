using Application.Abstractions.Messaging;
using Domain.Addresses;

namespace Application.Addresses.Create;

public record CreateAddressCommand(CreateAddressRequest request) : ICommand<Guid>;
