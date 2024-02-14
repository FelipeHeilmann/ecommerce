using Application.Abstractions.Messaging;
using Domain.Addresses;
using Domain.Shared;

namespace Application.Addresses.GetById;

public record GetAddressByIdQuery(Guid AddressId) : IQuery<Address>;
