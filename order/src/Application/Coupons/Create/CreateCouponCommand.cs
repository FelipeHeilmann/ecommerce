using Application.Abstractions.Messaging;

namespace Application.Coupons.Create;

public record CreateCouponCommand(string Name, DateTime ExpiressAt, double Value): ICommand<Guid>;
