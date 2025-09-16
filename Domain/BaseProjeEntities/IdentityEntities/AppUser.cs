using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Domain.BaseProjeEntities.IdentityEntities;

public class AppUser : IdentityUser<Guid>
{
    public string NameSurname { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}






