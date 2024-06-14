using Application.Abstractions.Messaging;

namespace Application.Customers.GetByOrderId;

public record GetCustomerByOrderIdQuery(Guid OrderId): IQuery<Output>;
public record Output(Guid Id, string Name, string Email, string CPF, string Phone, DateTime BirthDate);
