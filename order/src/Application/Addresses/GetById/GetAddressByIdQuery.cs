using Application.Abstractions.Messaging;

namespace Application.Addresses.GetById;

public record GetAddressByIdQuery(Guid AddressId) : IQuery<Output>;
public record Output(Guid Id, Guid CustomerId, string ZipCode, string Steet, string Neighborhood, string Number, string? Complement, string City, string State, string Country);
