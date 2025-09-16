namespace Application.AuthorizeAndAuthentication.CustomAttributes.DTOs;

public class EndpointRoleAssignment
{
    public string Endpoint { get; set; } // Endpoint'in kodu
    public List<string> Roles { get; set; } // Roller
    public string Menu { get; set; } // Menü adı
}
