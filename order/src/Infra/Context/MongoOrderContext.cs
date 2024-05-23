using Application.Abstractions.Query;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;

namespace Infra.Context;

public class MongoOrderContext : IOrderQueryContext
{
    public IMongoCollection<Infra.Models.MongoDB.OrderModel> _orderCollection { get; }

    public MongoOrderContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("ConnectionStrings:Mongo"));

        var database = client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDatabase"));

        _orderCollection = database.GetCollection<Infra.Models.MongoDB.OrderModel>(configuration.GetValue<string>("ConnectionStrings:CollectionName"));
    }

    public async Task<ICollection<OrderQueryModel>> GetAll()
    {
        var orders = await _orderCollection.Find(o => true).ToListAsync();
        return orders.Select(o => new OrderQueryModel()
        {
            Id = Guid.Parse(o.OrderId),
            CustomerId = Guid.Parse(o.CustomerId),
            PayedAt = o.PayedAt,
            Status = o.Status,
            Total = o.Items.Sum(i => i.Price * i.Quantity),
            Items = o.Items.Select(li => new LineItemQueryModel()
            {
                Id = Guid.Parse(li.Id),
                Description = li.Description,
                ImageUrl = li.ImageUrl,
                Name = li.Name,
                Price = li.Price,
                ProductId = Guid.Parse(li.ProductId),
                Quantity = li.Quantity,
            }).ToList()
        }).ToList();
    }

    public async Task<ICollection<OrderQueryModel>> GetByCustomerId(Guid id)
    {
        var orders = await _orderCollection.Find(o => o.CustomerId == id.ToString()).ToListAsync();
        return orders.Select(o => new OrderQueryModel()
        {
            Id = Guid.Parse(o.OrderId),
            CustomerId = Guid.Parse(o.CustomerId),
            PayedAt = o.PayedAt,
            Status = o.Status,
            Total = o.Items.Sum(i => i.Price * i.Quantity),
            Items = o.Items.Select(li => new LineItemQueryModel()
            {
                Id = Guid.Parse(li.Id),
                Description = li.Description,
                ImageUrl = li.ImageUrl,
                Name = li.Name,
                Price = li.Price,
                ProductId = Guid.Parse(li.ProductId),
                Quantity = li.Quantity,
            }).ToList()
        }).ToList();
    }

    public async Task<OrderQueryModel> GetById(Guid id)
    {
        var order = await _orderCollection.Find(o => o.OrderId == id.ToString()).FirstOrDefaultAsync();
        return new OrderQueryModel()
        {
            Id = Guid.Parse(order.OrderId),
            CustomerId = Guid.Parse(order.CustomerId),
            PayedAt = order.PayedAt,
            Status = order.Status,
            Total = order.Items.Sum(i => i.Price * i.Quantity),
            Items = order.Items.Select(li => new LineItemQueryModel()
            {
                Id = Guid.Parse(li.Id),
                Description = li.Description,
                ImageUrl = li.ImageUrl,
                Name = li.Name,
                Price = li.Price,
                ProductId = Guid.Parse(li.ProductId),
                Quantity = li.Quantity,
            }).ToList()
        };
    }

    public async Task Save(OrderQueryModel model)
    {
        var orderModel = new Models.MongoDB.OrderModel()
        {
            CustomerId = model.CustomerId.ToString(),
            OrderId = model.Id.ToString(),
            PayedAt = model.PayedAt,
            Status = model.Status,
            Items = model.Items.Select(i => new Models.MongoDB.LineItemModel()
            {
                Id = i.Id.ToString(),
                Description = i.Description,
                ImageUrl = i.ImageUrl,
                Name = i.Name,
                Price = i.Price,
                ProductId = i.ProductId.ToString(),
                Quantity = i.Quantity,
            }).ToList(),
        };

        await _orderCollection.InsertOneAsync(orderModel);
    }

    public async Task Update(OrderQueryModel model)
    {

        var update = Builders<Models.MongoDB.OrderModel>.Update
            .Set(o => o.Status, model.Status)
            .Set(o => o.PayedAt, model.PayedAt);

        await _orderCollection.UpdateOneAsync(o => o.OrderId == model.Id.ToString(), update);
    }
}
