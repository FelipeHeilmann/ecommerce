using Domain.Addresses.VO;
using Domain.Shared;

namespace Domain.Addresses.Entity;

public class Address
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string ZipCode { get => _zipCode.Value; private set => _zipCode = VO.ZipCode.Create(value).Value; }
    public ZipCode _zipCode;
    public string Street { get; private set; }
    public string Neighborhood { get; private set; }
    public string Number { get; private set; }
    public string? Complement { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }

    public Address() { }
    public Address(
        Guid id,
        Guid customerId,
        ZipCode zipCode,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
        Id = id;
        CustomerId = customerId;
        _zipCode = zipCode;
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        Country = country;
    }

    public static Result<Address> Create(
        Guid customerId,
        string zipCodeString,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
        var zipCode = VO.ZipCode.Create(zipCodeString);

        if (zipCode.IsFailure) return Result.Failure<Address>(zipCode.Error);

        return new Address(
            Guid.NewGuid(),
            customerId,
            zipCode.Value,
            street,
            neighborhood,
            number,
            complement,
            city,
            state,
            country
        );
    }

    public Result Update(
        string zipCodeString,
        string street,
        string neighborhood,
        string number,
        string? complement,
        string city,
        string state,
        string country
    )
    {
        var zipCode = VO.ZipCode.Create(zipCodeString);

        if (zipCode.IsFailure) return Result.Failure<Address>(zipCode.Error);

        _zipCode = zipCode.Value;
        Street = street;
        Neighborhood = neighborhood;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        Country = country;

        return Result.Success();

    }

}
