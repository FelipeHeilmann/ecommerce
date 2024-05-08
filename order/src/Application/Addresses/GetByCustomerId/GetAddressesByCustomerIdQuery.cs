using Application.Abstractions.Messaging;
using Domain.Addresses;

namespace Application.Addresses.GetByCustomerId;

public record GetAddressesByCustomerIdQuery(Guid CustomerId) : IQuery<ICollection<Output>>;
public record Output(Guid Id, Guid CustomerId, string ZipCode, string Steet, string Neighborhood, string Number, string? Complement, string City, string State, string Country);
