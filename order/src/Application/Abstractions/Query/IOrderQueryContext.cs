using Domain.Orders.Entity;

namespace Application.Abstractions.Query;

public interface IOrderQueryContext
{
    Task<ICollection<OrderQueryModel>> GetAll();
    Task<OrderQueryModel> GetById(Guid id);
    Task Update(OrderQueryModel order);
    Task<ICollection<OrderQueryModel>> GetByCustomerId(Guid id);
    Task Save(OrderQueryModel model);
}
