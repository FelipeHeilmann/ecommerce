using Application.Abstractions.Query;

namespace Infra.Context;

public class MemoryOrderContext : IOrderQueryContext
{
    private readonly ICollection<OrderQueryModel> orders;
    
    public MemoryOrderContext()
    {
        orders = new List<OrderQueryModel>();   
    }

    public Task<ICollection<OrderQueryModel>> GetAll()
    {
       return Task.FromResult(orders);
    }

    public Task<ICollection<OrderQueryModel>> GetByCustomerId(Guid id)
    {
        return Task.FromResult<ICollection<OrderQueryModel>>(orders.Where(o => o.CustomerId == id).ToList());
    }

    public Task<OrderQueryModel?> GetById(Guid id)
    {
        return Task.FromResult(orders.FirstOrDefault(o => o.Id == id));
    }
}
