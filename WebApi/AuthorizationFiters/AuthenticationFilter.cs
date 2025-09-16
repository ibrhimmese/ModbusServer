using Application.ExceptionTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthenticationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.User.Identity!.IsAuthenticated)
        {
            throw new AuthorizationException();
        }

        await next();
    }
}
