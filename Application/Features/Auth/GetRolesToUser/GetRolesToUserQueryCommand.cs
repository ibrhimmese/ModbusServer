using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.GetRolesToUser;

public class GetRolesToUserQueryCommand:IRequest<GetRolesToUserQueryCommandResponse>
{
    public string UserName { get; set; }
}

public class GetRolesToUserQueryCommandHandler(IUserService userService) : IRequestHandler<GetRolesToUserQueryCommand, GetRolesToUserQueryCommandResponse>
{
    async Task<GetRolesToUserQueryCommandResponse> IRequestHandler<GetRolesToUserQueryCommand, GetRolesToUserQueryCommandResponse>.Handle(GetRolesToUserQueryCommand request, CancellationToken cancellationToken)
    {
       var datas = await userService.GetRolesToUserAsync(request.UserName);

        return new()
        {
            UserId = request.UserName,
            UserRoles = datas
        };
    }
}
