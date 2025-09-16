using Domain.BaseProjeEntities.CoreEntities;

namespace Domain.BaseProjeEntities.IdentityEntities;

public class Endpoint : Entity<Guid>
{
    public string ActionType { get; set; }
    public string HttpType { get; set; }
    public string Definition { get; set; }
    public string Code { get; set; }
    public Menu Menu { get; set; }
    public ICollection<AppRole> Roles { get; set; }

    public Endpoint()
    {
        Roles = new List<AppRole>();
    }

}

