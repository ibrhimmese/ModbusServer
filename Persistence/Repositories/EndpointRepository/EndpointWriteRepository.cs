using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Domain.BaseProjeEntities.IdentityEntities;
using Persistence.Contexts;
using Persistence.GenericRepositories;

namespace Persistence.Repositories.EndpointRepository;

public class EndpointWriteRepository : WriteRepository<Endpoint, Guid, BaseDbContext>, IEndpointWriteRepository
{
    public EndpointWriteRepository(BaseDbContext context) : base(context)
    {
    }
}
