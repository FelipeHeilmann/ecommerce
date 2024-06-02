using API.Extensions;
using API.Middleware;
using Application.Orders.AddItemToCart;
using Application.Orders.Cancel;
using Application.Orders.Checkout;
using Application.Orders.GetByCustomerId;
using Application.Orders.GetById;
using Application.Orders.GetCart;
using Application.Orders.Model;
using Application.Orders.RemoveItemRemoveItemFromCart;
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

        var query = new GetOrdersByCustomerIdQuery(customerId.Value);

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

    [ServiceFilter(typeof(ApiKeyAuthenticationMiddleware))]
    [HttpGet("service/{id}")]
    public async Task<IResult> GetByIdFromService(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }


    [Authorize]
    [HttpGet("cart")]
    public async Task<IResult> GetCart(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCartQuery(), cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : result.Value.Id is null ? Results.Ok(new { }) : Results.Ok(result.Value);
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<IResult> Checkout([FromBody] CheckoutOrderRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        var command = new CheckoutOrderCommand(request.Items, customerId!.Value, request.ShippingAddressId, request.BillingAddressId, request.PaymentType, request.CardToken, request.Installments);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("cart/remove-item/{lineItemId}")]
    public async Task<IResult> RemoveLineItem(Guid lineItemId, CancellationToken cancellationToken)
    {
        var command = new RemoveItemFromCartCommand(lineItemId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }

    [Authorize]
    [HttpPatch("cart/add-item")]
    public async Task<IResult> AddLineItem([FromBody] AddItemRequest request, CancellationToken cancellationToken)
    {
        var customerId = GetCustomerId();

        var command = new AddItemToCartCommand(customerId.GetValueOrDefault(), request.ProductId, request.Quantity);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }

    [Authorize]
    [HttpPatch("{id}/cancel")]
    public async Task<IResult> CancelrOrder(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CancelOrderCommand(id), cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }
}


