using Microsoft.AspNetCore.Identity;

namespace Domain.BaseProjeEntities.IdentityEntities;

public class AppRole : IdentityRole<Guid>
{
    public ICollection<Endpoint> Endpoints { get; set; }
}

