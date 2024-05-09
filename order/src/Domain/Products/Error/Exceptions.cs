using Domain.Shared;

namespace Domain.Products.Error;

public class InvalidSku : BaseException
{
    public InvalidSku() : base ("Invalid.Sku", "The sku value is invalid", 400) {}
}
