using Application.Abstractions.Messaging;
using Application.Addresses.Model;

namespace Application.Addresses.Update;

public record UpdateAddressCommand(UpdateAddressRequest request): ICommand;
