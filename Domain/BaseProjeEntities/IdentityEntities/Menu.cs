using Domain.BaseProjeEntities.CoreEntities;

namespace Domain.BaseProjeEntities.IdentityEntities;

public class Menu : Entity<Guid>
{
    public string Name { get; set; }

    public ICollection<Endpoint> Endpoints { get; set; }
}

