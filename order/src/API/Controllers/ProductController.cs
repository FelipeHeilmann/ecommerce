using API.Extensions;
using Application.Products.GetAll;
using Application.Products.GetById;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    }
}
