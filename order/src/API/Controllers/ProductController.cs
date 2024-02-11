using API.Extensions;
using Application.Products.Create;
using Application.Products.Delete;
using Application.Products.GetAll;
using Application.Products.GetById;
using Application.Products.Model;
using Application.Products.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : APIBaseController
    {
        public ProductController(ISender _sender, IHttpContextAccessor _contextAccessor) : base(_sender, _contextAccessor) { }

        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllProductsQuery();

            var result = await _sender.Send(query, cancellationToken);

            return Results.Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(id);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsFailure ? result.ToProblemDetail() :  Results.Ok(result.Value);
        }

        [HttpPost]
        public async Task<IResult> Create([FromBody] CreateProductModel request, CancellationToken cancellationToken)
        {
            var query = new CreateProductCommand(request);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsFailure ? result.ToProblemDetail() : Results.Created<Guid>($"/products/{result.Value}", result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update(Guid id, [FromBody] CreateProductModel request, CancellationToken cancellationToken)
        {
            var query = new UpdateProductCommand(
                new UpdateProductModel(
                    id, 
                    request.Name, 
                    request.Description, 
                    request.Currency, 
                    request.Price, 
                    request.ImageUrl, 
                    request.Sku, 
                    request.CategoryId)
                );

            var result = await _sender.Send(query, cancellationToken);

            return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(id);

            var result = await _sender.Send(command, cancellationToken);

            return result.IsFailure ? result.ToProblemDetail() : Results.NoContent();
        }
    }
}
