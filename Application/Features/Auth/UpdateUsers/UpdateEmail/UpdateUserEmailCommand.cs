using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.UpdateUsers.UpdateEmail;

public class UpdateUserEmailCommand:IRequest<bool>
{
    public string Email { get; set; }
}

public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand, bool>
{
    private readonly IUserService _userService;

    public UpdateUserEmailCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
    {
        await _userService.UpdateEmailAsync(request.Email,cancellationToken);
        return true;
    }
}