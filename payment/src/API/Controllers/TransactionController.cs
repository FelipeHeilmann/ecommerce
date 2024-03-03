using Application.Transactions.MakePaymentRequest;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/transactions")]
[ApiController]
public class TransactionController : APIBaseController
{
    public TransactionController(ISender _sender) : base(_sender) { }

    [HttpPost]
    public async Task<IResult> CreateTransaction([FromBody ]OrderPurchasedEvent request, CancellationToken cancellationToken)
    {
        var command = new CreatePaymentCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return Results.Ok(result.Value);
    }
    
}
