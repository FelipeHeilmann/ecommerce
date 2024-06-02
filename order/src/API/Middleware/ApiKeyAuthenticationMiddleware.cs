using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class ApiKeyAuthenticationMiddleware : Attribute, IAuthorizationFilter
{
    private const string APIKEYNAME = "ApiKey";
    private readonly string _apiKey;

    public ApiKeyAuthenticationMiddleware(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>(APIKEYNAME) ?? throw new ArgumentException();
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
