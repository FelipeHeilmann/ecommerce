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

        var result = await _sender.Send(query);

        return Results.Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(Guid id,CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(id);

        var result = await _sender.Send(query);

        return result.IsFailure ? result.ToProblemDetail() :  Results.Ok(result.Value);
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CategoryModel request, CancellationToken cancellationToken)
    {
        var query = new CreateCategoryCommand(request);

        var result = await _sender.Send(query);

        return result.IsFailure ? result.ToProblemDetail() : Results.Created<Guid>($"/categories/{result.Value}", result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IResult> Create(Guid id, [FromBody] CategoryModel request, CancellationToken cancellationToken)
    {
        var query = new UpdateCategoryCommand(new UpdateCategoryModel(request.Name, request.Description, id));

        var result = await _sender.Send(query);

        return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
    }
}
