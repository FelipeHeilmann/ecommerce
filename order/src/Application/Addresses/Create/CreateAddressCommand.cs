using Application.Abstractions.Messaging;
using Application.Addresses.Model;

namespace Application.Addresses.Create;

public record CreateAddressCommand(CreateAddressRequest request) : ICommand<Guid>;
