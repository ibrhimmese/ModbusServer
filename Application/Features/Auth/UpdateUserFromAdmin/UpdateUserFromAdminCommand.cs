using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.UpdateUserFromAdmin;

public class UpdateUserFromAdminCommand:IRequest<bool>
{
    public Guid UserId { get; set; }
    public string NameSurname { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class UpdateUserFromAdminCommandHandler : IRequestHandler<UpdateUserFromAdminCommand, bool>
{
    private readonly IUserService userService;

    public UpdateUserFromAdminCommandHandler(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<bool> Handle(UpdateUserFromAdminCommand request, CancellationToken cancellationToken)
    {
        
        await userService.UpdateUserFromAdminAsync(
            request.UserId,
            request.NameSurname,
            request.UserName,
            request.Email,
            request.NewPassword,
            request.ConfirmNewPassword,
            cancellationToken);

        return true;
    }
}
