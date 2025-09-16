using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.Pipelines;
using MediatR;


namespace Application.Features.Auth.Login;

public class LoginUserCommand : IRequest<LoginUserCommandResponse>
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }

    public LoginUserCommand(string userNameOrEmail, string password)
    {
        UserNameOrEmail = userNameOrEmail;
        Password = password;
    }
}


public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
{
    private readonly IAuthService authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        this.authService = authService;
    }
    public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {

        var token = await authService.LoginAsync(request.UserNameOrEmail, request.Password, 60);
        return new LoginUserCommandSuccessResponse()
        {
            Token = token
        };
    }
}
