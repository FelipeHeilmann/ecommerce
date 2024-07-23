
using Application.Abstractions.Messaging;

namespace Application.Orders.Delivery;
public record DeliveryOrderCommand(Guid OrderId) : ICommand;