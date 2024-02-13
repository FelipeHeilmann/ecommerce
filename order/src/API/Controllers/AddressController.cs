using API.Extensions;
using Application.Addresses.Create;
using Domain.Addresses;
using Domain.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/addresses")]
[ApiController]
public class AddressController : APIBaseController
{
    public AddressController(ISender sender) : base(sender) { }

    [Authorize]
    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateAddressHttpRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        var createAddressRequest = new CreateAddressRequest(
                customerId.Value,
                request.Zipcode,
                request.Street,
                request.Neighborhood,
                request.Number,
                request.Apartment,
                request.City,
                request.State,
                request.Country
        );

        var command = new CreateAddressCommand(createAddressRequest);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created($"/addresses/{result.Value}", result.Value);
    }
}
