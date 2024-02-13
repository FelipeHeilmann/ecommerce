using Domain.Shared;

namespace Domain.Address;

public static class AddressErrors
{
    public static Error InvalidFormat => Error.Validation("Zip.Code.Format", "The zipcode is invalid");

    public static Error ZipCodeNull => Error.Validation("Zip.Code.Null.Empty", "The zipcode value should not be empty or null");
}
