using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectionWorker.Model;

public class OrderModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("order_id")]
    public string OrderId { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("items")]
    public ICollection<ItemModel> Items { get; set; } = new List<ItemModel>();
    
    [BsonElement("payment")]
    public PaymentModel? Payment { get; set; }

    [BsonElement("address")]
    public AddressModel? Address { get; set; }

    [BsonElement("customer_id")]
    public string CustomerId { get; set; } = string.Empty;

}

public class ItemModel
{
    [BsonElement("id")]
    public string? Id { get; set; }

    [BsonElement("product_id")]
    public string ProductId { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("price")]
    public double Price { get; set; }

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}

public class AddressModel
{
    [BsonElement("id")]
    public string Id { get; set; } = string.Empty;

    [BsonElement("zip_code")]
    public string ZipCode { get; set; } = string.Empty;

    [BsonElement("street")]
    public string Street { get; set; } = string.Empty;

    [BsonElement("neighborhood")]
    public string Neighborhood { get; set; } = string.Empty;

    [BsonElement("number")]
    public string Number { get; set; } = string.Empty;

    [BsonElement("complement")]
    public string? Complement { get; set; }

    [BsonElement("state")]
    public string State { get; set; } = string.Empty;

    [BsonElement("city")]
    public string City { get; set; } = string.Empty;

    [BsonElement("country")]
    public string Country { get; set; } = string.Empty;
}
public class PaymentModel
{
    [BsonElement("payment_type")]
    public string PaymentType { get; set; } = string.Empty;

    [BsonElement("installments")]
    public int Installments { get; set; }

    [BsonElement("payed_at")]
    public DateTime? PayedAt { get; set; }
}
