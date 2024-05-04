using API.Extensions;
using Application.Customers.Create;
using Application.Customers.Login;
using Application.Customers.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController : APIBaseController
{
    public CustomerController(ISender sender)
    : base(sender) { }

    [HttpPost]
    public async Task<IResult> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created<Guid>($"/customers/{result.Value}", result.Value);
    }

    [HttpPost("auth")]
    public async Task<IResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(new {Token = result.Value });
    }
}
