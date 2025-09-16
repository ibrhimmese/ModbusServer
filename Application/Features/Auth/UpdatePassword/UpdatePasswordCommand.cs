using Application.ExceptionTypes;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.UpdatePassword;

public class UpdatePasswordCommand:IRequest<UpdatePasswordCommandResponse>
{
    public string UserId { get; set; }
    public string ResetToken { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
}

public class UpdatePasswordCommandHandler(IUserService userService) : IRequestHandler<UpdatePasswordCommand, UpdatePasswordCommandResponse>
{
    public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.Password.Equals(request.PasswordConfirm))
        {
            throw new BusinessException("Şifreleriniz örtüşmemektedir");
        }
        await userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);

        return new();
    }
}