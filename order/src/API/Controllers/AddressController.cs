﻿using API.Extensions;
using API.Middleware;
using Application.Addresses.Create;
using Application.Addresses.GetByCustomerId;
using Application.Addresses.GetById;
using Application.Addresses.Model;
using Application.Addresses.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/addresses")]
[ApiController]
public class AddressController : APIBaseController
{
    public AddressController(ISender sender, IHttpContextAccessor contextAccessor) : base(sender, contextAccessor) { }

    [Authorize]
    [HttpGet]
    public async Task<IResult> GetByCustomerId(CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        var query = new GetAddressesByCustomerIdQuery(customerId!.Value);

        var result = await _sender.Send(query, cancellationToken);

        return Results.Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    { 
        var query = new GetAddressByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() :  Results.Ok(result.Value);
    }

    [Authorize]
    [HttpPost]
    public async Task<IResult> Create([FromBody] AddressRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        var command = new CreateAddressCommand(
                customerId!.Value,
                request.Zipcode,
                request.Street,
                request.Neighborhood,
                request.Number,
                request.Complement,
                request.City,
                request.State,
                request.Country
        );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created($"/addresses/{result.Value}", new { Id = result.Value });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IResult> Update(Guid id, [FromBody] AddressRequest request, CancellationToken cancellationToken) 
    {
        var customerId = GetCustomerId();

        var command = new UpdateAddressCommand(
                id,
                customerId!.Value,
                request.Zipcode,
                request.Street,
                request.Neighborhood,
                request.Number,
                request.Complement,
                request.City,
                request.State,
                request.Country
            );

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }

    [ServiceFilter(typeof(ApiKeyAuthenticationMiddleware))]
    [HttpGet("service/{id}")]
    public async Task<IResult> GetByIdWithApiKey(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAddressByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }
}
