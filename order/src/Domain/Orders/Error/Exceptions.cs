using Domain.Shared;

namespace Domain.Orders.Error
{
    public class LineItemNotFound : BaseException
    {
        public LineItemNotFound() : base("LineItem.Not.Found", "The line item was not found", 404){}
    }

    public class CannotRemoveItem : BaseException 
    {
        public CannotRemoveItem(): base("Cant.Remove.Item","The order is not a cart and has just 1 item", 420) { }
    }
}
