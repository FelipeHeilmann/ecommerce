using API.Extensions;
using API.Requests;
using Application.Coupons.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/coupons")]
[ApiController]
public class CouponController : APIBaseController
{
    public CouponController(ISender sender, IHttpContextAccessor contextAccessor) : base(sender, contextAccessor) { }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateCouponRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCouponCommand(request.Name, DateTime.SpecifyKind(DateTime.Parse(request.ExpiressAt), DateTimeKind.Utc), request.Value);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }
}
