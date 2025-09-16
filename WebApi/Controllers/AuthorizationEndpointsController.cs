
using Application.AuthorizeAndAuthentication.CustomAttributes.DTOs;
using Application.Features.Auth.CreateAssignRoleToEndpoint;
using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationEndpointsController : BaseController
{
    private readonly IEndpointReadRepository _endpointReadRepository;
   

    public AuthorizationEndpointsController(IEndpointReadRepository endpointReadRepository)
    {
      
        _endpointReadRepository = endpointReadRepository;
    }


    [HttpPost]
    public async Task<IActionResult> AuthorizationEndpoints([FromBody]AssignRoleCommand assignRoleCommand)
    {
        assignRoleCommand.Type = typeof(Program);
        var response = await Mediator.Send(assignRoleCommand);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetEndpointRoleAssignmentsAsync(CancellationToken cancellationToken)
    {
        var endpointRoleAssignments = await _endpointReadRepository.Query()
            .Include(e => e.Menu)
            .Include(e => e.Roles) // Endpoint ile ilişkili rolleri dahil et
            .Select(e => new EndpointRoleAssignment
            {
                Endpoint = e.Code,
                Menu = e.Menu.Name,
                Roles = e.Roles.Select(r => r.Name).ToList() // Rollerin isimlerini al
            })
            .ToListAsync(cancellationToken);

        return Ok(endpointRoleAssignments); // IActionResult döndürmek için Ok() kullanılıyor
    }
}
