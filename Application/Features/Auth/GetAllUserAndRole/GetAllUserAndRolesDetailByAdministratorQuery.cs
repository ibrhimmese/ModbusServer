using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.GetAllUserAndRole;

public class GetAllUserAndRolesDetailByAdministratorQuery : IRequest<List<ListUserAndRoleDetailsDto>>
{
}


public class GetAllUserAndRolesDetailByAdministratorQueryHandler : IRequestHandler<GetAllUserAndRolesDetailByAdministratorQuery, List<ListUserAndRoleDetailsDto>>
{
    private readonly IUserService _userService;
    public GetAllUserAndRolesDetailByAdministratorQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<List<ListUserAndRoleDetailsDto>> Handle(GetAllUserAndRolesDetailByAdministratorQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAllUsersAndRolesDetailByAdministratorAsync();
    }
}