using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.ChangePassword;

public class ChangePasswordCommand:IRequest<bool>
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
}


public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IAuthService _authService;

    public ChangePasswordCommandHandler(IAuthService userService)
    {
        _authService = userService;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(request.CurrentPassword, request.NewPasswordConfirm, request.NewPassword,cancellationToken);

        return true;
    }
}