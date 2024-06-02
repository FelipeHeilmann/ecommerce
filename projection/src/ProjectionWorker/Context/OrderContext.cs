using MongoDB.Driver;
using ProjectionWorker.Model;

namespace ProjectionWorker.Context;

public class OrderContext
{
    public IMongoCollection<OrderModel> _orderCollection { get; }

    public OrderContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("ConnectionStrings:Mongo"));

        var database = client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDatabase"));

        _orderCollection = database.GetCollection<OrderModel>(configuration.GetValue<string>("ConnectionStrings:CollectionName"));
    }

    public async Task<OrderModel> GetById(Guid id)
    {
        var order = await _orderCollection.Find(o => o.OrderId == id.ToString()).FirstOrDefaultAsync();
        if (order == null) throw new Exception("Order not found");
        return order;
    }
    
    public async Task Save(OrderModel model)
    {
        await _orderCollection.InsertOneAsync(model);
    }

    public async Task Update(OrderModel model)
    {

        var update = Builders<OrderModel>.Update
            .Set(o => o.Status, model.Status)
            .Set(o => o.Payment.PayedAt, model.Payment.PayedAt);

        await _orderCollection.UpdateOneAsync(
                o => o.OrderId == model.OrderId.ToString(), update);
    }
}

