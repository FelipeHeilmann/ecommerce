﻿using API.Extensions;
using Application.Orders.AddLineItem;
using Application.Orders.Checkout;
using Application.Orders.Create;
using Application.Orders.GetByCustomerId;
using Application.Orders.GetById;
using Application.Orders.GetCart;
using Application.Orders.Model;
using Application.Orders.RemoveItem;
using Domain.Orders.Entity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : APIBaseController
{
    public OrderController(ISender _sender) : base(_sender) { }

    [Authorize]
    [HttpGet]
    public async Task<IResult> GetAll(CancellationToken cancellation)
    {
        var customerId = GetCustomerId();

        if (customerId == null) return Results.Unauthorized();

        var query = new GetOrdersByCustomerQuery(customerId.Value);

        var result = await _sender.Send(query, cancellation);

        return Results.Ok(result.Value);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{id}/checkout")]
    public async Task<IResult> Checkout(Guid id, [FromBody] CheckoutOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CheckoutOrderCommand(id, request.ShippingAddressId, request.BillingAddressId, request.PaymentType, request.CardToken, request.Installments);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IResult> Create([FromBody] List<OrderItemRequest> request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        if (customerId == null) return Results.Unauthorized();

        var command = new CreateOrderCommand(new OrderRequest(request, customerId.Value));

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created($"/orders/{result.Value}", new { Id = result.Value });
    }

    [Authorize]
    [HttpPatch("cart/remove-item/{lineItemId}")]
    public async Task<IResult> RemoveLineItem(Guid lineItemId ,CancellationToken cancellationToken)
    {
        var command = new RemoveItemFromCartCommand(lineItemId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }

    [Authorize]
    [HttpPatch("cart/add-item")]
    public async Task<IResult> AddLineItem( [FromBody] AddItemRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();
        
        var command = new AddItemToCartCommand(customerId.GetValueOrDefault(), request.ProductId, request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }

    [Authorize]
    [HttpGet("cart")]
    public async Task<IResult> GetCart(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCartQuery(), cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : result.Value.Id is null ? Results.Ok(new {}) : Results.Ok(result.Value);
    }
}


