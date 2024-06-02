using Domain.Orders.Entity;

namespace Application.Abstractions.Query;

public interface IOrderQueryContext
{
    Task<ICollection<OrderQueryModel>> GetAll();
    Task<OrderQueryModel> GetById(Guid id);
    Task<ICollection<OrderQueryModel>> GetByCustomerId(Guid id);
}
