using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.VerifyResetToken;


public class VerifyResetTokenCommand:IRequest<VerifyResetTokenResponse>
{
    public string ResetToken { get; set; }
    public string UserId { get; set; }
}

public class VerifyResetTokenCommandHandler : IRequestHandler<VerifyResetTokenCommand, VerifyResetTokenResponse>
{
    private readonly IAuthService _authService;

    public VerifyResetTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<VerifyResetTokenResponse> Handle(VerifyResetTokenCommand request, CancellationToken cancellationToken)
    {
       bool state = await _authService.VerifyResetTokenAsync(request.ResetToken,request.UserId);

        return new()
        {
            State = state,
        };
    }
}