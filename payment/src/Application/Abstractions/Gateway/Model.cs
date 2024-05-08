using Domain.Events;

namespace Application.Abstractions.Gateway;

public record ProccessPaymentRequest(
     Guid OrderId, 
     double Total, 
     IEnumerable<LineItemOrderPurchased> Items, 
     Guid CustomerId, 
     string CustomerName,
     string CustomerEmail, 
     string CustomerDocument, 
     string CustomerPhone, 
     string PaymentType, 
     string CardToken, 
     int Installment, 
     string AddressZipCode, 
     string AddressNumber, 
     string? AddressLine 
);
