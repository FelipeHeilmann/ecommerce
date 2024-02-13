using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.Addresses;

public record ZipCode
{
    public string Value { get; set; }

    private ZipCode(string value) => Value = value;

    public static Result<ZipCode> Create(string value) 
    {
        if (string.IsNullOrEmpty(value)) return Result.Failure<ZipCode>(AddressErrors.ZipCodeNull);

        var Rgx = new Regex(@"^\d{5}-\d{3}$");

        if (!Rgx.IsMatch(value)) return Result.Failure<ZipCode>(AddressErrors.InvalidFormat);

        return new ZipCode(value);
    }
}
