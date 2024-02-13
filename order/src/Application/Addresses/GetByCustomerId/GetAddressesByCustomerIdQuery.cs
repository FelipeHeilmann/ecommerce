using Application.Abstractions.Messaging;
using Domain.Addresses;

namespace Application.Addresses.GetByCustomerId;

public record GetAddressesByCustomerIdQuery(Guid CustomerId) : IQuery<ICollection<Address>>;
