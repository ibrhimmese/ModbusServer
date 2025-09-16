using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Auth.Login;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenLoginCommand:IRequest<LoginUserCommandResponse>
{
    public string RefreshToken { get; set; }
}

public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommand, LoginUserCommandResponse>
{
    private readonly IAuthService _authService;
    public RefreshTokenLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<LoginUserCommandResponse> Handle(RefreshTokenLoginCommand request, CancellationToken cancellationToken)
    {
        var token = await _authService.RefreshTokenLoginAsync(request.RefreshToken);
        return new LoginUserCommandSuccessResponse()
        {
            Token = token
        };
    }
}