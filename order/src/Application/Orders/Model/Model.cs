namespace Application.Orders.Model;

public record OrderRequestModel(List<OrderItemRequestModel> OrderItens);

public record OrderItemRequestModel(Guid ProductId, int Quantity);
