using Application.Abstractions.Messaging;

namespace Application.Orders.GetCart;

public record GetCartQuery : IQuery<Output?>;
public record Output
{
    public Guid? Id { get; init; }
    public Guid? CustomerId { get; init; }
    public string? Status { get; init; }
    public IEnumerable<ItemsOutput>? Items { get; init; }
    public double? Total { get; init; }
    public Guid? BillingAddressId { get; init; }
    public Guid? ShippingAddressId { get; init; }

    public Output(Guid? id, Guid? customerId, string? status, IEnumerable<ItemsOutput>? items, double? total ,Guid? billingAddressId, Guid? shippingAddressId)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
        Items = items;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        Total = total;
    }
}
public record ItemsOutput(Guid LineItemId, Guid ProductId, double Price, int Quantity);
