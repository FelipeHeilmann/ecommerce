using Domain.Products.Error;
using Domain.Shared;
namespace Domain.Products.VO;

public record Sku
{
    public const int DefaultLength = 15;
    public string Value { get; init; }

    public Sku(string sku)
    {
        if (string.IsNullOrEmpty(sku)) throw new InvalidSku();

        if (sku.Length > DefaultLength) throw new InvalidSku();

        Value = sku;
    }
}