namespace Application.Features.Auth.GetRolesToUser;

public class GetRolesToUserQueryCommandResponse
{
    public string UserId { get; set; }
    public string[] UserRoles { get; set; }
}