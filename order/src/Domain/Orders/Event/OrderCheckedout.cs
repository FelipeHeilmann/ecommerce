using MediatR;

namespace Domain.Orders.Events;


public record OrderCheckedout(Guid OrderId) : INotification;

