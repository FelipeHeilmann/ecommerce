using Application.Abstractions.Messaging;
using Application.Data;
using Domain.Coupons.Entity;
using Domain.Coupons.Repository;
using Domain.Shared;

namespace Application.Coupons.Create;

public class CreateCouponCommandHandler : ICommandHandler<CreateCouponCommand, Guid>
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCouponCommandHandler(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = Coupon.Create(command.Name, command.Value, command.ExpiressAt);
        _couponRepository.Add(coupon);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(coupon.Id);   
    }
}
