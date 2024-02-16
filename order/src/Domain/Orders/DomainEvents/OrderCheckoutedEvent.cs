using Domain.Addresses;
using Domain.Customers;

namespace Domain.Orders.DomainEvents;

public record OrderCheckoutedEvent(
    Order Order, 
    Customer Customer, 
    string PaymentType,
    string? CardToken,
    int Installment, 
    Address BillingAddress,
    Address ShipingAddress
);
