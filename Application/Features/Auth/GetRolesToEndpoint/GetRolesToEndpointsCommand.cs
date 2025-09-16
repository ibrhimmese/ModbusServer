using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.GetRolesToEndpoint;

public class GetRolesToEndpointsCommand:IRequest<GetRolesToEndpointsCommandResponse>
{
    public string Code { get; set; }
    public string Menu { get; set; }
}

public class GetRolesToEndpointsCommandHandler(IAuthorizationEndpointService authorizationEndpointService) : IRequestHandler<GetRolesToEndpointsCommand, GetRolesToEndpointsCommandResponse>
{
    public async Task<GetRolesToEndpointsCommandResponse> Handle(GetRolesToEndpointsCommand request, CancellationToken cancellationToken)
    {
        var datas = await authorizationEndpointService.GetRolesToEndpointAsync(request.Code,request.Menu);
        return new()
        {
            Roles = datas
        };
    }
}
