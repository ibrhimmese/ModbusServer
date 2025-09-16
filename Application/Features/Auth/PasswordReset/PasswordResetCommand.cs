
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.PasswordReset;

public class PasswordResetCommand:IRequest<PasswordResetCommandResponse>
{
    public string Email { get; set; }
}

public class PasswordResetCommandHandler : IRequestHandler<PasswordResetCommand, PasswordResetCommandResponse>
{

    private readonly IAuthService authService;

    public PasswordResetCommandHandler(IAuthService authService)
    {
        this.authService = authService;
    }

    public async Task<PasswordResetCommandResponse> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
    {
       await authService.PasswordResetAsync(request.Email);
        return new();
    }
}
