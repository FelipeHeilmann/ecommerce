using Domain.Addresses.Error;
using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.Addresses.VO;

public record ZipCode
{
    public string Value { get; private set; }

    public ZipCode(string zipCode) 
    {
        if (string.IsNullOrEmpty(zipCode)) throw new InvalidZipCode();

        var Rgx = new Regex(@"^\d{5}-\d{3}$");

        if (!Rgx.IsMatch(zipCode)) throw new InvalidZipCode(); ;

        Value = zipCode;
    }
}
