using System.Data;
using Domain.Coupons.Entity;
using Domain.Orders.Entity;
using Domain.Orders.Repository;
using Domain.Products.VO;
using Infra.Database;

namespace Infra.Repositories.Database;

public class OrderRepositoryDatabase : IOrderRepository
{
    private readonly IDatabaseConnection _connection;

    public OrderRepositoryDatabase(IDatabaseConnection connection)
    {
        _connection = connection;
    }

    public async Task<ICollection<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await _connection.Query("select o.*, c.name as coupon_name, c.value as coupon_value, c.expires_at as coupon_expires_at from orders o left join coupons c on o.coupon_id = c.id", null, MapEmptyOrder);
        foreach(var order in orders)
        {
            if(order is not null) 
            {
                var lineItems = await _connection.Query("select * from line_item where order_id = @OrderId", new { OrderId = order.Id }, MapLineItem);
                order.RestoreLineItems(lineItems.ToList());
            }
        }    
        return orders.ToList();
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var orders = await _connection.Query("select o.*, c.name as coupon_name, c.value as coupon_value, c.expires_at as coupon_expires_at from orders o left join coupons c on o.coupon_id = c.id where o.id = @Id", new { Id = id }, MapEmptyOrder);
        var order = orders.FirstOrDefault();
        if(order is not null) 
        {
            var lineItems = await _connection.Query("select * from line_item where order_id = @OrderId", new { OrderId = id }, MapLineItem);
            order.RestoreLineItems(lineItems.ToList());
        }
        return order;
    }

    public async Task<Order?> GetCart(CancellationToken cancellationToken)
    {
        var orders = await _connection.Query("select o.*, c.name as coupon_name, c.value as coupon_value, c.expires_at as coupon_expires_at from orders o left join coupons c on o.coupon_id = c.id where o.status = @Status", new { Status = "cart" }, MapEmptyOrder);
        var order = orders.FirstOrDefault();       
        if(order is not null) 
        {
            var lineItems = await _connection.Query("select * from line_item where order_id = @OrderId", new { OrderId = order.Id }, MapLineItem);
            order.RestoreLineItems(lineItems.ToList());
        }
        return order;
    }

    public async Task<ICollection<Order>> GetOrdersByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var orders = await _connection.Query("select o.*, c.name as coupon_name, c.value as coupon_value, c.expires_at as coupon_expires_at from orders o left join coupons c on o.coupon_id = c.id where o.customer_id = @CustomerId", new { CustomerId = customerId }, MapEmptyOrder);
        foreach(var order in orders)
        {
            if(order is not null) 
            {
                var lineItems = await _connection.Query("select * from line_item where order_id = @OrderId", new { OrderId = order.Id }, MapLineItem);
                order.RestoreLineItems(lineItems.ToList());
            }
        }    
        return orders.ToList();;
    }

    public IQueryable<Order> GetQueryable(CancellationToken cancellationToken)
    {
        var orders = _connection.Query("select o.*, c.name as coupon_name, c.value as coupon_value, c.expires_at as coupon_expires_at from orders o left join coupons c on o.coupon_id = c.id", null, MapEmptyOrder).Result;
        foreach(var order in orders)
        {
            if(order is not null) 
            {
                var lineItems = _connection.Query("select * from line_item where order_id = @OrderId", new { OrderId = order.Id }, MapLineItem).Result;
                order.RestoreLineItems(lineItems.ToList());
            }
        }    
        return orders.AsQueryable();
    }

    public async Task Add(Order entity)
    {
        var orderParams = new
        {
            entity.Id,
            entity.Status, 
            entity.CustomerId,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.BillingAddressId,
            entity.ShippingAddressId,
            entity.CouponId
        };
        await _connection.Query<Task>("insert into orders (id, status, customer_id, created_at, updated_at, billing_address_id, shipping_address_id, coupon_id) values (@Id, @Status, @CustomerId, @CreatedAt, @UpdatedAt, @BillingAddressId, @ShippingAddressId, @CouponId)", orderParams);
        foreach(var item in entity.Items) 
        {
            await _connection.Query<Task>("insert into line_item (id, order_id, product_id, quantity, currency, amount) values (@Id, @OrderId, @ProductId, @Quantity, @Currency, @Amount)",item);
        }
    }

    public async Task Update(Order entity)
{
    var currentOrder = await GetByIdAsync(entity.Id, CancellationToken.None);
    if (currentOrder is null)
    {
        throw new InvalidOperationException("Order not found");
    }

    var orderParams = new 
    {
        entity.Id,
        entity.Status,
        entity.UpdatedAt,
        entity.BillingAddressId,
        entity.ShippingAddressId,
        entity.CouponId
    };
    
    await _connection.Query<Task>("UPDATE orders SET status = @Status, updated_at = @UpdatedAt, billing_address_id = @BillingAddressId, shipping_address_id = @ShippingAddressId, coupon_id = @CouponId WHERE id = @Id", orderParams);

    var currentLineItems = currentOrder.Items.ToList();
    var newItems = entity.Items.Where(item => !currentLineItems.Any(ci => ci.Id == item.Id)).ToList();
    var removedItems = currentLineItems.Where(item => !entity.Items.Any(ei => ei.Id == item.Id)).ToList();
    var updatedItems = entity.Items.Where(item => currentLineItems.Any(ci => ci.Id == item.Id && ci.Quantity != item.Quantity)).ToList();

    foreach (var newItem in newItems)
    {
        await _connection.Query<Task>(
            "INSERT INTO line_item (id, order_id, product_id, quantity, currency, amount) VALUES (@Id, @OrderId, @ProductId, @Quantity, @Currency, @Amount)", 
            new {
                newItem.Id,
                newItem.OrderId,
                newItem.ProductId,
                newItem.Quantity,
                newItem.Currency,
                newItem.Amount
        });
    }

    foreach (var removedItem in removedItems)
    {
        await _connection.Query<Task>("DELETE FROM line_item WHERE id = @Id",new { removedItem.Id });
    }

    foreach (var updatedItem in updatedItems)
    {
        await _connection.Query<Task>("UPDATE line_item SET quantity = @Quantity  WHERE id = @Id", new { updatedItem.Quantity, updatedItem.Id,  });
    }
}

    public async Task Delete(Order entity)
    {
        //TODO - transaction
        await _connection.Query<Task>("delete from line_item where order_id = @OrderId", new { OrderId = entity.Id });
        await _connection.Query<Task>("delete from orders where id = @Id", new { entity.Id });
    }

    private Order MapEmptyOrder(IDataReader reader)
    {
        return Order.Restore(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetGuid(reader.GetOrdinal("customer_id")),
            reader.GetString(reader.GetOrdinal("status")),
            new List<LineItem>(),
            reader.IsDBNull(reader.GetOrdinal("coupon_id")) ? null : Coupon.Restore(
                            reader.GetGuid(reader.GetOrdinal("coupon_id")),
                            reader.GetString(reader.GetOrdinal("coupon_name")),
                            reader.GetDouble(reader.GetOrdinal("coupon_value")),
                            reader.GetDateTime(reader.GetOrdinal("coupon_expires_at"))
                        ),
            reader.GetDateTime(reader.GetOrdinal("created_at")),
            reader.GetDateTime(reader.GetOrdinal("updated_at")),
            reader.IsDBNull(reader.GetOrdinal("billing_address_id")) ? null : reader.GetGuid(reader.GetOrdinal("billing_address_id")),
           reader.IsDBNull(reader.GetOrdinal("shipping_Address_id")) ? null : reader.GetGuid(reader.GetOrdinal("shipping_Address_id"))
        );
    }

    private LineItem MapLineItem(IDataReader reader)
    {
        return new LineItem(
            reader.GetGuid(reader.GetOrdinal("id")),
            reader.GetGuid(reader.GetOrdinal("order_id")),
            reader.GetGuid(reader.GetOrdinal("product_id")),
            new Money(reader.GetString(reader.GetOrdinal("currency")), reader.GetDouble(reader.GetOrdinal("amount"))),
            reader.GetInt32(reader.GetOrdinal("quantity"))
        );
    }
    
}
