using Domain.Shared;
using System.Text.RegularExpressions;

namespace Domain.Customer;

public record Email
{
    public string Value { get; init; }

    private Email(string email) => Value = email;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return Result.Failure<Email>(CustomerErrors.EmailNull);

        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        if (!regex.Match(email).Success) return Result.Failure<Email>(CustomerErrors.EmailFormat);

        return new Email(email);
    }
}
