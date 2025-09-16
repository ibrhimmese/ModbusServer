using Application.CustomAttributes;
using Application.ExceptionTypes;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace ServerAPI.AuthorizationFiters
{
    public class RolePermissionFilter(IUserService userService,IConfiguration configuration) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name =context.HttpContext.User.Identity?.Name;
           // if(!string.IsNullOrEmpty(name) && name != "Super")
            if(!string.IsNullOrEmpty(name) && name != configuration["PrivateVariables:SuperAdminUserName"])
                {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;

                var httpMethodAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute)) as HttpMethodAttribute;

                var code = $"{(httpMethodAttribute != null ? httpMethodAttribute.HttpMethods.First() : HttpMethods.Get)}.{attribute?.ActionType}.{attribute?.Definition.Replace(" ","")}";

                var hasRole =await userService.HasRolePermissionToEndpointAsync(name,code);

                if (!hasRole)
                {
                    if (!context.HttpContext.User.Identity.IsAuthenticated)
                        throw new AuthorizationException(); 
                    else
                    throw new AuthenticationException();
                }
                else
                {
                    await next();
                }
               
            }
            else
                await next();
        }
    }
}
