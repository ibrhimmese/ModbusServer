using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.UpdateUsers.UpdateUserName;

public class UpdateUserUserNameCommand:IRequest<bool>
{
    public string UserName { get; set; }
}

public class UpdateUserUserNameCommandHandler : IRequestHandler<UpdateUserUserNameCommand, bool>
{
    private readonly IUserService _userService;
    public UpdateUserUserNameCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<bool> Handle(UpdateUserUserNameCommand request, CancellationToken cancellationToken)
    {
        await _userService.UpdateUserNameAsync(request.UserName, cancellationToken);
        return true;
    }
}