using API.Extensions;
using Application.Customers.Create;
using Application.Customers.GetById;
using Application.Customers.GetByOrderId;
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
    public async Task<IResult> CreateAccount([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCustomerCommand(request.Name, request.Email, request.password, request.birthDate, request.CPF, request.Phone);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created($"/customers/{result.Value}", new { Id = result.Value });
    }

    [HttpPost("auth")]
    public async Task<IResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(new {Token = result.Value });
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }

    [HttpGet("orders/{id}")]
    public async Task<IResult> GetByOrderId(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByOrderIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }
}
