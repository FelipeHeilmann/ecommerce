using Application.Categories.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : APIBaseController
    {
        public CategoryController(ISender _sender, IHttpContextAccessor _contextAccessor) : base(_sender, _contextAccessor) { }

        [HttpGet]
        public async Task<IResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllCategoriesQuery();

            var result = await _sender.Send(query);

            return Results.Ok(result.Value);
        }
    }
}
