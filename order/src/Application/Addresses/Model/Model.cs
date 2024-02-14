namespace Application.Addresses.Model;

public record CreateAddressRequest(
        Guid CustomerId, 
        string Zipcode, 
        string Street, 
        string Neighborhood, 
        string Number,
        string? Apartment,
        string City,
        string State, 
        string Country
    );

public record UpdateAddressRequest(
        Guid Id,
        Guid CustomerId,
        string Zipcode,
        string Street,
        string Neighborhood,
        string Number,
        string? Apartment,
        string City,
        string State,
        string Country
    );

public record AddressRequest(
        string Zipcode,
        string Street,
        string Neighborhood,
        string Number,
        string? Apartment,
        string City,
        string State,
        string Country);
