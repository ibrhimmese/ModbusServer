using Domain.BaseProjeEntities.IdentityEntities;

namespace Application.JwtTokenHandlerInterface.DTOs;

public class ListUserDto
{
    public Guid Id { get; set; }
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool TwoFactorEnabled { get; set; }
}

 public class ListUserAndRoleDto
{
    public Guid Id { get; set; }
    public List<AppRoleNameDto> Roles { get; set; }
}
public class ListUserAndRoleDetailsDto
{
    public Guid Id { get; set; }
    public string NameSurname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<AppRoleNameDto> Roles { get; set; }
}

 public class AppRoleNameDto
{
    public string Name { get; set; }
}
       