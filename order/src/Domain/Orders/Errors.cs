using Domain.Shared;

namespace Domain.Orders
{
    public static class OrderErrors
    {
        public static Error OrderNotFound => Error.NotFound("Order.Not.Found", "The order was not found");
        public static Error OrderHasOneLineItem => Error.Validation("Order.Has.One.Line.Item", "The order has one line item, you can remove the item");
        public static Error LineItemNotFound => Error.NotFound("Line.Not.Found", "The line item was not found");
        public static Error CartNotFound => Error.NotFound("Cart.Not.Found", "The cart was not found");
        public static Error OrderStatusCouldNotBeProccessed => Error.Validation("Order.Status.Invalid", "The order could not be processed due its status");
        public static Error OrderDoesnotHaveCustomerId => Error.Validation("Order.Customer.Id", "The order could no be canceled because it does not have customer id");
    }
}
