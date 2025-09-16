using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using MediatR;

namespace Application.Features.Auth.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
}


public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserService userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Kullanıcıyı sil
        await userService.DeleteUserAsync(request.UserId, cancellationToken);

        // Başarılı yanıt döndür
        return true;
    }
}