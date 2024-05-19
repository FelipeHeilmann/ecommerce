using API.Extensions;
using Application.Categories.Create;
using Application.Categories.GetAll;
using Application.Categories.GetById;
using Application.Categories.Model;
using Application.Categories.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : APIBaseController
{
    public CategoryController(ISender _sender) : base(_sender) { }

    [HttpGet]
    public async Task<IResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return Results.Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id,CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() :  Results.Ok(result.Value);
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name, request.Description);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created($"/categories/{result.Value}", new { Id = result.Value } );
    }

    [HttpPut("{id}")]
    public async Task<IResult> Update(Guid id, [FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(request.Name, request.Description, id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }
}
