using Domain.Customers.Error;

namespace Domain.Customers.VO;

public record Name
{
    public string Value { get; init; }

    public Name(string name) 
    {
        if (string.IsNullOrEmpty(name)) throw new InvalidName();
        if (name.Split(" ").Length < 2) throw new InvalidName();

        Value = name;
    }

}
