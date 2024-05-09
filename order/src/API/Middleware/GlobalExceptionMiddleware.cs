using Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public class GlobalExceptionMiddleware 
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(BaseException ex)
        {
            var problemDetail = new ProblemDetails()
            {
                Status = ex.StatusCode ?? 500,
                Title = ex.Message,
                Extensions = new Dictionary<string, object?>
                {
                    { "error", new[] { new { ex.Code, ex.Description  } } }
                }
            };

            context.Response.StatusCode = problemDetail.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problemDetail);
        }
        catch (Exception ex)
        {
            var problemDetail = new ProblemDetails()
            {
                Status = 500,
                Title = ex.Message,
                Extensions = new Dictionary<string, object?>
                {
                    { "error", new[] {ex.Message} }
                }
            };

            context.Response.StatusCode = problemDetail.Status ?? 500;
            await context.Response.WriteAsJsonAsync(problemDetail);
        }
    }
}
