namespace Application.Orders.Model;

public record OrderRequest(List<OrderItemRequest> OrderItens, Guid CustomerId);

public record OrderItemRequest(Guid ProductId, int Quantity);

public record AddItemRequest(int Quantity);
