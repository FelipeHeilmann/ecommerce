using Application.Abstractions.Messaging;
using Domain.Coupons.Entity;
using Domain.Coupons.Repository;
using Domain.Shared;

namespace Application.Coupons.Create;

public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, Guid>
{
    private readonly ICouponRepository _couponRepository;

    public CreateCouponCommandHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = Coupon.Create(command.Name, command.Value, command.ExpiressAt);
        await _couponRepository.Add(coupon);
        return Result.Success(coupon.Id);   
    }
}
