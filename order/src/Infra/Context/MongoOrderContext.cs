using Application.Abstractions.Query;
using Domain.Orders.Entity;
using Domain.Orders.Error;
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
            Address = new AddressQueryModel()
            {
                Id = Guid.Parse(o.Address.Id),
                City = o.Address.City,
                ZipCode = o.Address.ZipCode,
                Complement = o.Address.Complement,
                Country = o.Address.Country,
                Neighborhood = o.Address.Neighborhood,
                Number = o.Address.Number,
                State = o.Address.State,
                Street = o.Address.Street
            },
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
        if (order == null) throw new OrderNotFound();
        return new OrderQueryModel()
        {
            Id = Guid.Parse(order.OrderId),
            CustomerId = Guid.Parse(order.CustomerId),
            PayedAt = order.PayedAt,
            Status = order.Status,
            Total = order.Items.Sum(i => i.Price * i.Quantity),
            Address = new AddressQueryModel()
            {
                Id = Guid.Parse(order.Address.Id),
                City = order.Address.City,
                ZipCode = order.Address.ZipCode,
                Complement = order.Address.Complement,
                Country = order.Address.Country,
                Neighborhood = order.Address.Neighborhood,
                Number = order.Address.Number,
                State = order.Address.State,    
                Street = order.Address.Street
            },
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
            Address = new Models.MongoDB.AddressModel()
            {
                Id = model.Id.ToString(),
                City = model.Address.City,
                ZipCode = model.Address.ZipCode,
                Complement = model.Address.Complement,
                Country = model.Address.Country,
                Neighborhood = model.Address.Neighborhood,
                Number = model.Address.Number,
                State = model.Address.State,
                Street = model.Address.Street
            },
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
