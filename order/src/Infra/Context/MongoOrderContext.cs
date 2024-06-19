using Application.Abstractions.Query;
using Domain.Orders.Entity;
using Domain.Orders.Error;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

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
            Payment = new PaymentQueryModel()
            {
                PayedAt = o.Payment?.PayedAt,
                Installments = o.Payment!.Installments,
                PaymentType = o.Payment.PaymentType,
            },
            Status = o.Status,
            Total = o.Items.Sum(i => i.Price * i.Quantity),
            Items = o.Items.Select(li => new LineItemQueryModel()
            {
                Id = Guid.Parse(li.Id!),
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
            Payment = o.Payment == null ? null : new PaymentQueryModel()
            {
                PayedAt = o.Payment.PayedAt,
                Installments = o.Payment.Installments,
                PaymentType = o.Payment.PaymentType,
            },
            Status = o.Status,
            Total = o.Items.Sum(i => i.Price * i.Quantity),
            Address = o.Address == null ? null : new AddressQueryModel()
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
                Id = Guid.Parse(li.Id!),
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
            Payment = order.Payment == null ? null : new PaymentQueryModel()
            {
                PayedAt = order.Payment.PayedAt,
                Installments = order.Payment.Installments,
                PaymentType = order.Payment.PaymentType,
            },
            Status = order.Status,
            Total = order.Items.Sum(i => i.Price * i.Quantity),
            Address = order.Address == null ? null : new AddressQueryModel()
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
                Id = Guid.Parse(li.Id!),
                Description = li.Description,
                ImageUrl = li.ImageUrl,
                Name = li.Name,
                Price = li.Price,
                ProductId = Guid.Parse(li.ProductId),
                Quantity = li.Quantity,
            }).ToList()
        };
    }
}
