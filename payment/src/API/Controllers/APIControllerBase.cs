using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
public abstract class APIBaseController : ControllerBase
{
    protected readonly ISender _sender;
    protected readonly IHttpContextAccessor _contextAccessor;

    protected APIBaseController(ISender sender)
    {
        _sender = sender;
    }

    protected Guid? GetCustomerId()
    {
        var user = HttpContext.User;

        if (!user.Identity.IsAuthenticated) return null;

        var id = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.Parse(id);
    }

}
