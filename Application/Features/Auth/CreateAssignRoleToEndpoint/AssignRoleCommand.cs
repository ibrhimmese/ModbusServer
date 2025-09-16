using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;

using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Auth.CreateAssignRoleToEndpoint;

public class AssignRoleCommand : IRequest<AssignRoleCommandResponse>
{
    public string[] Roles { get; set; }
    public string Code { get; set; }
    public string Menu { get; set; }

    [JsonIgnore]
    public Type? Type { get; set; }
}

public class AssignRoleCommandHandler(IAuthorizationEndpointService authorizationEndpointService) : IRequestHandler<AssignRoleCommand, AssignRoleCommandResponse>
{
    public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        await authorizationEndpointService.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code, request.Type);

        return new() { };
    }
}
