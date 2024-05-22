using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infra.Models.MongoDB;

public class OrderModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("order_id")]
    public string OrderId { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("items")]
    public ICollection<LineItemModel> Items { get; set; }

    [BsonElement("customer_id")]
    public string CustomerId { get; set; }

    [BsonElement("payed_at")]
    public DateTime? PayedAt { get; set; }
}

public class LineItemModel
{
    [BsonElement("id")]
    public string? Id { get; set; }

    [BsonElement("product_id")]
    public string ProductId { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("price")]
    public double Price { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("image_url")]
    public string ImageUrl {  get; set; } 

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}
