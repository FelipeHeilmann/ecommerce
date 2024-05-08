namespace Domain.Orders.Error;

public static class OrderErrors
{
    public static Shared.Error OrderNotFound => Shared.Error.NotFound("Order.Not.Found", "The order was not found");
    public static Shared.Error OrderHasOneLineItem => Shared.Error.Validation("Order.Has.One.Line.Item", "The order has one line item, you can remove the item");
    public static Shared.Error LineItemNotFound => Shared.Error.NotFound("Line.Not.Found", "The line item was not found");
    public static Shared.Error CartNotFound => Shared.Error.NotFound("Cart.Not.Found", "The cart was not found");
    public static Shared.Error OrderStatusCouldNotBeProccessed => Shared.Error.Validation("Order.Status.Invalid", "The order could not be processed due its status");
    public static Shared.Error OrderDoesnotHaveCustomerId => Shared.Error.Validation("Order.Customer.Id", "The order could no be canceled because it does not have customer id");
}
