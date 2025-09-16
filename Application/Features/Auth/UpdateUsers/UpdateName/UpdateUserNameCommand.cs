using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.UpdateUsers.UpdateName;

public class UpdateUserNameCommand:IRequest<bool>
{
    public string Name { get; set; }
}

public class UpdateUserNameCommandHandler : IRequestHandler<UpdateUserNameCommand, bool>
{
    private readonly IUserService _userService;
    public UpdateUserNameCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<bool> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {
        await _userService.UpdateNameAsync(request.Name, cancellationToken);
        return true;
    }
}
