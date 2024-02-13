namespace Domain.Addresses;

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

public record CreateAddressHttpRequest(
        string Zipcode,
        string Street,
        string Neighborhood,
        string Number,
        string? Apartment,
        string City,
        string State,
        string Country);
