using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Customers.GetById
{
    public record GetCustomerByIdQuery(Guid customerId): IQuery<Customer>;
}
