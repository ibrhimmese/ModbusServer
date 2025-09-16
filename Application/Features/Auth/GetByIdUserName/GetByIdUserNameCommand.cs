using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.GetByIdUserName;

public class GetByIdUserNameCommand:IRequest<GetByIdUserNameCommandResponse>
{
    public Guid Id { get; set; }
}

public class GetByIdUserNameCommandHandler : IRequestHandler<GetByIdUserNameCommand, GetByIdUserNameCommandResponse>
{
    private readonly IUserService _userService;
    public GetByIdUserNameCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<GetByIdUserNameCommandResponse> Handle(GetByIdUserNameCommand request, CancellationToken cancellationToken)
    {
        string usernameSurname = await _userService.GetByIdUserNameAsync(request.Id);

        return new GetByIdUserNameCommandResponse()
        {
            NameSurname = usernameSurname
        };
    }
}