using Domain.Customers.Error;
using Domain.Shared;

namespace Domain.Customers.VO;

public record Name
{
    public string Value { get; init; }

    private Name(string name) => Value = name;

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrEmpty(name)) return Result.Failure<Name>(CustomerErrors.NameFormat);
        if (name.Split(" ").Length < 2) return Result.Failure<Name>(CustomerErrors.NameFormat);

        return new Name(name);
    }
}
