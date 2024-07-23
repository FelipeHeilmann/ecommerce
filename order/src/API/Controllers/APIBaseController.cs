using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

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

    protected Guid? GetCustomerId()
    {
        var user = HttpContext.User;

        if (user?.Identity?.IsAuthenticated != true) return null;

        var id = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return id != null ? Guid.Parse(id) : null;
    }

}
