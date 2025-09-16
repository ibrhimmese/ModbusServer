namespace Application.JwtTokenHandlerInterface.DTOs;

public class CreateUserDTO
{
    public string NameSurname { get; set; } 
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }
    public Guid? TenantId { get; set; }

    public CreateUserDTO()
    {
        NameSurname = string.Empty;
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        PasswordConfirm = string.Empty;
    }


}
