using Domain.BaseProjeEntities.IdentityEntities;
using Persistence.Contexts;
using Persistence.GenericRepositories;
using Application.Interfaces.RepositoryServices.MenuRepositories;

namespace Persistence.Repositories.MenuRepository;

public class MenuWriteRepository : WriteRepository<Menu, Guid, BaseDbContext>, IMenuWriteRepository
{
    public MenuWriteRepository(BaseDbContext context) : base(context)
    {
    }
}

