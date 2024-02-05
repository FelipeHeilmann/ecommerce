using Domain.Shared;
namespace Domain.Products;

public record Sku
{
    public const int DefaultLength = 15;
    public string Value { get; init; } 

    private Sku(string sku) => Value = sku;

    public static Result<Sku> Create(string sku)
    {
        if (string.IsNullOrEmpty(sku)) return Result.Failure<Sku>(ProductErrors.SkuNull);
                
        if(sku.Length > DefaultLength) return Result.Failure<Sku>(ProductErrors.SkuLength); ;

        return new Sku(sku);
    }
}
