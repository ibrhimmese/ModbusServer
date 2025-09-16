using Domain.BaseProjeEntities.IdentityEntities;
using Persistence.Contexts;
using Persistence.GenericRepositories;
using Application.Interfaces.RepositoryServices.EndpointRepositories;

namespace Persistence.Repositories.EndpointRepository;

public class EndpointReadRepository : ReadRepository<Endpoint, Guid, BaseDbContext>, IEndpointReadRepository
{
    public EndpointReadRepository(BaseDbContext context) : base(context)
    {
    }
}

