using Domain.Shared;

namespace Domain.Addresses.Error;

public static class AddressErrors
{
    public static Shared.Error InvalidFormat => Shared.Error.Validation("Zip.Code.Format", "The zipcode is invalid");
    public static Shared.Error NotFound => Shared.Error.Validation("Address.Not,Found", "The address was not found");
    public static Shared.Error ZipCodeNull => Shared.Error.Validation("Zip.Code.Null.Empty", "The zipcode value should not be empty or null");
}
