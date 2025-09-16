using Domain.BaseProjeEntities.IdentityEntities;
using Persistence.Contexts;
using Persistence.GenericRepositories;
using Application.Interfaces.RepositoryServices.MenuRepositories;

namespace Persistence.Repositories.MenuRepository;

public class MenuReadRepository : ReadRepository<Menu, Guid, BaseDbContext>, IMenuReadRepository
{
    public MenuReadRepository(BaseDbContext context) : base(context)
    {
    }
}

