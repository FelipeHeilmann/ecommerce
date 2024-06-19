using Application.Abstractions.Query;
using Domain.Orders.Error;

namespace Infra.Context;

public class MemoryOrderContext : IOrderQueryContext
{
    private List<OrderQueryModel> orders;
    
    public MemoryOrderContext()
    {
        orders = [];   
    }

    public Task<ICollection<OrderQueryModel>> GetAll()
    {
       return Task.FromResult<ICollection<OrderQueryModel>>(orders);
    }

    public Task<ICollection<OrderQueryModel>> GetByCustomerId(Guid id)
    {
        return Task.FromResult<ICollection<OrderQueryModel>>(orders.Where(o => o.CustomerId == id).ToList());
    }

    public Task<OrderQueryModel> GetById(Guid id)
    {
        var order  = orders.FirstOrDefault(o => o.Id == id);
        if(order == null) throw new OrderNotFound();
        return Task.FromResult(order);
    }
}
