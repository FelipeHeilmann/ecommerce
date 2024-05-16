using Domain.Orders.Entity;
using Domain.Orders.Repository;
using Infra.Context;
using Infra.Models;
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
        foreach(var orderModel in _context.Set<OrderModel>().Include("Items").ToList())
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.AsQueryable();
    }

    public async Task<ICollection<Order>> GetAllAsync(CancellationToken cancellationToken, string? include = null)
    {
        var orders = new List<Order>();
        foreach (var orderModel in await _context.Set<OrderModel>().Include(li => li.Items).ToListAsync())
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.ToList();
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken, string? include = null)
    {
        var orderModel = await _context.Set<OrderModel>().Include("Items").FirstOrDefaultAsync(model => model.Id == id);
        return orderModel?.ToAggregate(); 
    }

    public async Task<Order?> GetCart(CancellationToken cancellationToken, string? includes = null)
    {
        var orderModel = await _context.Set<OrderModel>().Include(li => li.Items).FirstOrDefaultAsync(model => model.Status == "cart");
        return orderModel?.ToAggregate();
    }

    public async Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken, string? includes = null)
    {
        var orderModels = await _context.Set<OrderModel>().Where(model => model.CustomerId ==  customerId).Include("Items").ToListAsync();
        var orders = new List<Order>();
        foreach (var orderModel in orderModels)
        {
            orders.Add(orderModel.ToAggregate());
        }
        return orders.ToList();
    }

    public void Add(Order entity)
    {
        _context.Add(OrderModel.FromAggreate(entity));
    }


    public void Update(Order entity)
    {
        _context.Update(OrderModel.FromAggreate(entity));
    }

    public void Delete(Order entity)
    {
       _context.Remove(OrderModel.FromAggreate(entity));
    }
}
