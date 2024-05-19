using Application.Abstractions.Messaging;

namespace Application.Addresses.Update;

public record UpdateAddressCommand(Guid Id,
        Guid CustomerId,
        string Zipcode,
        string Street,
        string Neighborhood,
        string Number,
        string? Complement,
        string City,
        string State,
        string Country) : ICommand;
