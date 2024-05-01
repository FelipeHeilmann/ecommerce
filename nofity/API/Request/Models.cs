namespace API.Request
{
    public record WelcomeRequest(string Email, string Name);
    public record OrderCreatedRequest(Guid OrderId, DateTime Date, string Name, string Email, List<ItemsRequest> Items);
    public record ItemsRequest(string Name, double Price, int Quantity);
    public record PaymenRecivedRequest(string Name, string Email, Guid OrderId, double Amount);
}
