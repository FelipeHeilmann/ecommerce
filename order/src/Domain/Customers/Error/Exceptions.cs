using Domain.Shared;

namespace Domain.Customers.Error;

public class InvalidCPF : BaseException
{
    public InvalidCPF() : base("Invalid.CPF", "The cpf value is invalid", 400) {}
}

public class InvalidEmail : BaseException
{
    public InvalidEmail() : base("Invalid.Email", "The email value is invalid", 400) { }
}

public class InvalidPasswordLenght : BaseException
{
    public InvalidPasswordLenght() : base("Invalid.Password.Lenght", "Password length must be greater than 5", 400){ }
}

public class InvalidPhone : BaseException
{
    public InvalidPhone() : base("Invalid.Phone", "The phone value is invalid", 400) { }
}

public class InvalidName : BaseException
{
    public InvalidName() : base("Invalid.Name", "The name value is invalid", 400) { }
}

public class UnderAge : BaseException
{
    public UnderAge() : base("Invalid.Age", "Age should be greatter than 18 years", 400) {}
}



