using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.DTOs;
using MediatR;

namespace Application.Features.Auth.GetCurrentUser;

public class GetCurrentUserQuery:IRequest<ListUserAndRoleDetailsDto>
{
}


public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, ListUserAndRoleDetailsDto>
{
    private readonly IUserService _userService;
    private readonly IUserIdService _userIdService;
    public GetCurrentUserQueryHandler(IUserService userService, IUserIdService userIdService)
    {
        _userService = userService;
        _userIdService = userIdService;
    }
    public async Task<ListUserAndRoleDetailsDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = await _userIdService.GetUserIdAsync(cancellationToken);
        return await _userService.GetCurrentUserDetailAsync(userId);
    }
}