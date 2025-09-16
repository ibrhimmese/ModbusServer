using Application.ExceptionTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]


public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    //protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? throw new OperationException("IMediator service is not registered.");

}
