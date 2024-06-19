using API.Models;
using Application.Transactions.ProccessTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : APIBaseController
{
    public TransactionController(ISender _sender, IHttpContextAccessor contextAccessor) : base(_sender, contextAccessor) { }

    [HttpPost("payment-proccessed")]
    public async Task<IResult> ProccessPayment([FromBody] PaymentWebHookModel request,  CancellationToken cancellationToken)
    {
        var command = new ProccessTransactionCommand(request.Id, request.Status.ToLower());

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? Results.BadRequest() : Results.Ok();
    }
}
