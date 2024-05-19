using Application.Abstractions.Messaging;

namespace Application.Addresses.Create;

public record CreateAddressCommand(Guid CustomerId,
        string Zipcode,
        string Street,
        string Neighborhood,
        string Number,
        string? Complement,
        string City,
        string State,
        string Country) 
    : ICommand<Guid>;
