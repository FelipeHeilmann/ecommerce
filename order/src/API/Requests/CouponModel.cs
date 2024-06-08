namespace API.Requests;

public record CreateCouponRequest(string Name, double Value, string ExpiressAt);
