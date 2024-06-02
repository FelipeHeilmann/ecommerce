namespace NotifyWorker.Events;


public record OrderCheckedout(
    Guid OrderId,
    double Total,
    IEnumerable<LineItemOrderCheckedout> Items,
    Guid CustomerId,
    string PaymentType,
    string? CardToken,
    int Installment,
    Guid AddressId
);

public record LineItemOrderCheckedout(Guid Id, Guid ProductId, int Quantity, double Price);