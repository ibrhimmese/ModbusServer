using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.Pipelines;
using MediatR;
namespace Application.Features.Auth.GetUsers;
public class UserListQueryCommand:IRequest<UserListQueryCommandResponse>
{
}

public class UserListQueryCommandHandler(IUserService userService) : IRequestHandler<UserListQueryCommand, UserListQueryCommandResponse>
{
    public async Task<UserListQueryCommandResponse> Handle(UserListQueryCommand request, CancellationToken cancellationToken)
    {
       var datas = await userService.GetAllUsersAsync();

        return new()
        {
            UserList = datas
        };
        
    }
}
