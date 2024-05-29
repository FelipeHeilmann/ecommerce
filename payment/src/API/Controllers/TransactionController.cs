using API.Models;
using Application.Transactions.ProccessTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : APIBaseController
{
    public TransactionController(ISender _sender) : base(_sender) { }

    [HttpPost("payment-proccessed")]
    public async Task<IResult> ProccessPayment([FromBody] PaymentWebHookModel request,  CancellationToken cancellationToken)
    {
        var command = new ProccessTransactionCommand(request.Id, request.Status.ToLower());

        var result = await _sender.Send(command);

        return Results.Ok();
    }
}
