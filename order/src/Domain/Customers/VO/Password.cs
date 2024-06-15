using System.Reflection.Metadata.Ecma335;
using Domain.Customers.Error;

namespace Domain.Customers.VO;

public abstract class Password
{
    protected string _value;
    public string Value => _value;

    public Password(string value) => _value = value;

    public abstract bool PasswordMatches(string password);
}

public class BcryptPassword : Password
{
    protected BcryptPassword(string password): base(password){}

    public static BcryptPassword Create(string password)
    {
        if(password.Length < 5) throw new InvalidPasswordLenght();
        return new BcryptPassword(BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt()));
    }

    public static BcryptPassword Restore(string password) => new BcryptPassword(password);

    public override bool PasswordMatches(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, _value);
    }
}
