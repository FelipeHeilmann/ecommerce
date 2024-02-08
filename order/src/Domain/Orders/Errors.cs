using Domain.Shared;

namespace Domain.Orders
{
    public static class OrderErrors
    {
        public static Error OrderNotFound => Error.NotFound("Order.Not.Found", "The order was not found");
        public static Error OrderHasOneLineItem => Error.Validation("Order.Has.One.Line.Item", "The order has one line item, you can remove the item");
        public static Error LineItemNotFound => Error.NotFound("Line.Not.Found", "The line item was not found");
    }
}
