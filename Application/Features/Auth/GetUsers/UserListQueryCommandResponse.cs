
using Application.JwtTokenHandlerInterface.DTOs;

namespace Application.Features.Auth.GetUsers;
public class UserListQueryCommandResponse
{
    public List<ListUserAndRoleDetailsDto> UserList { get; set; }
}