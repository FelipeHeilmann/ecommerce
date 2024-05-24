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

    [BsonElement("address")]
    public AddressModel Address { get; set; }

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

public class AddressModel
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("zip_code")]
    public string ZipCode { get; set; }

    [BsonElement("street")]
    public string Street { get; set; }

    [BsonElement("neighborhood")]
    public string Neighborhood { get; set; }

    [BsonElement("number")]
    public string Number { get; set; }

    [BsonElement("complement")]
    public string? Complement { get; set; }

    [BsonElement("state")]
    public string State { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("country")]
    public string Country { get; set; }
}
