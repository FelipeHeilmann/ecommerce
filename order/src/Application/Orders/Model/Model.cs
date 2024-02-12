namespace Application.Orders.Model;

public record OrderRequestModel(List<OrderItemRequestModel> OrderItens, Guid CustomerId);

public record OrderItemRequestModel(Guid ProductId, int Quantity);

public record AddItemBody(int Quantity);
