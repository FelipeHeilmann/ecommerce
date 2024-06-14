using Application.Abstractions.Messaging;

namespace Application.Customers.GetById;

public record GetCustomerByIdQuery(Guid customerId): IQuery<Output>;

public record Output(Guid Id, string Name, string Email, string CPF, string Phone, DateTime BirthDate);
