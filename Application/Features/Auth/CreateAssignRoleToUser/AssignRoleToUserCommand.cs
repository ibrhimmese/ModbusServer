using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.CreateAssignRoleToUser;

public class AssignRoleToUserCommand : IRequest<AssignRoleToUserCommandResponse>
{
    public Guid UserId { get; set; }
    public string[] Roles { get; set; }
}


public class AssignRoleToUserCommandHandler(IUserService userService) : IRequestHandler<AssignRoleToUserCommand, AssignRoleToUserCommandResponse>
{
    public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        await userService.AssignRoleToUserAsync(request.UserId.ToString(), request.Roles);

        return new();
    }
}
