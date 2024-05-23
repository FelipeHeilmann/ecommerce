using MediatR;

namespace Domain.Orders.Event;

public record OrderCanceledEvent(Guid OrderId) : INotification;
