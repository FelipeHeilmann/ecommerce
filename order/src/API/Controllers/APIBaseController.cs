using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    public abstract class APIBaseController : ControllerBase
    {
        protected readonly ISender _sender;
        protected readonly IHttpContextAccessor _contextAccessor;

        protected APIBaseController(ISender sender, IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _contextAccessor = contextAccessor;
        }

    }
}
