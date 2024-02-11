using API.Extensions;
using Application.Customers.Create;
using Application.Customers.Login;
using Application.Customers.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : APIBaseController
    {
        public CustomerController(ISender sender, IHttpContextAccessor contextAccessor)
        : base(sender, contextAccessor) { }

        [HttpPost]
        public async Task<IResult> CreateAccount([FromBody] CreateAccountModel request, CancellationToken cancellationToken)
        {
            var command = new CreateAccountCommand(request);

            var result = await _sender.Send(command, cancellationToken);

            return result.IsFailure ? result.ToProblemDetail() : Results.Created<Guid>($"/transactions/{result.Value}", result.Value);
        }

        [HttpPost("auth")]
        public async Task<IResult> Login([FromBody] LoginModel request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request);

            var result = await _sender.Send(command);

            return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
        }
    }

}
