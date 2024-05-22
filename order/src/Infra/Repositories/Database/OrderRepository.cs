using Domain.Orders.Entity;
using Domain.Orders.Repository;
using Infra.Context;
using Infra.Models.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Database;

public class OrderRepository : IOrderRepository
{
    private readonly DbContext _context;
    public OrderRepository(ApplicationContext context) 
    {
        _context = context; 
    }

    public IQueryable<Order> GetQueryable(CancellationToken cancellation)
    {
        var orders = new List<Order>();
        foreach(var orderModel in _context.Set<OrderModel>().Include(model => model.Items).ToList())
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.AsQueryable();
    }

    public async Task<ICollection<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = new List<Order>();
        foreach (var orderModel in await _context.Set<OrderModel>().Include(model => model.Items).ToListAsync())
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.ToList();
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderModel = await _context.Set<OrderModel>().Include(model => model.Items).FirstOrDefaultAsync(model => model.Id == id);
        return orderModel?.ToAggregate(); 
    }

    public async Task<Order?> GetCart(CancellationToken cancellationToken)
    {
        var orderModel = await _context.Set<OrderModel>().Include(model => model.Items).FirstOrDefaultAsync(model => model.Status == "cart");
        return orderModel?.ToAggregate();
    }

    public async Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var orderModels = await _context.Set<OrderModel>().Where(model => model.CustomerId ==  customerId).Include(model => model.Items).ToListAsync();
        var orders = new List<Order>();
        foreach (var orderModel in orderModels)
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.ToList();
    }

    public void Add(Order entity)
    {
        _context.Add(OrderModel.FromAggregate(entity));
    }


    public void Update(Order entity)
    {
        var existingOrderModel = _context.Set<OrderModel>()
                                         .Include(o => o.Items)
                                         .FirstOrDefault(o => o.Id == entity.Id);

        if (existingOrderModel != null)
        {
            _context.Entry(existingOrderModel).State = EntityState.Detached;

            _context.Entry(OrderModel.FromAggregate(entity)).State = EntityState.Modified;

            foreach (var lineItem in entity.Items)
            {
                var lineItemModel = LineItemModel.FromAggregate(lineItem);
                var existingLineItemModel = _context.Set<LineItemModel>().FirstOrDefault(li => li.Id == lineItemModel.Id);

                if (existingLineItemModel != null)
                {

                    _context.Entry(existingLineItemModel).State = EntityState.Detached;

                    _context.Entry(lineItemModel).State = EntityState.Modified;
                }
                else
                {
                    _context.Entry(lineItemModel).State = EntityState.Added;
                }
            }

            var itemsToRemove = existingOrderModel.Items.Where(e => !entity.Items.Any(i => i.Id == e.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                existingOrderModel.Items.Remove(item);
                _context.Set<LineItemModel>().Remove(item);
            }
        }
        else
        {
            _context.Update(OrderModel.FromAggregate(entity));
        }
    }

        public void Delete(Order entity)
    {
       _context.Remove(OrderModel.FromAggregate(entity));
    }
}
