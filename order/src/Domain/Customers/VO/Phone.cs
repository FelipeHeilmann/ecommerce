using Domain.Customers.Error;
using System.Text.RegularExpressions;

namespace Domain.Customers.VO;

public record Phone
{
    public string Value { get; }

    public Phone(string phone) 
    {
        if (string.IsNullOrEmpty(phone)) throw new InvalidPhone();

        string cleanedPhone = Regex.Replace(phone, @"[^\d]", "");

        var regex = new Regex(@"^\d{2}\d{4,5}\d{4}$");

        if (!regex.IsMatch(cleanedPhone)) throw new InvalidPhone();

        Value = cleanedPhone;
           
    }
}
