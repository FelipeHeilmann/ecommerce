using Domain.Shared;

namespace Domain.Addresses.Error;

public class InvalidZipCode : BaseException
{
    public InvalidZipCode() : base("Invalid.ZipCode", "The zipcode value is invalid", 400) { }
} 
