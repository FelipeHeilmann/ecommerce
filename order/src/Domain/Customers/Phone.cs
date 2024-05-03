using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.Customers;

public record Phone
{
    public string Value { get; }

    private Phone(string phone) => Value = phone;

    public static Result<Phone> Create(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            return Result.Failure<Phone>(CustomerErrors.PhoneFormat);

        
        string cleanedPhone = Regex.Replace(phone, @"[^\d]", "");

        var regex = new Regex(@"^\d{2}\d{4,5}\d{4}$");

        if (!regex.IsMatch(cleanedPhone))
            return Result.Failure<Phone>(CustomerErrors.PhoneFormat);

        return new Phone(cleanedPhone);
    }
}
