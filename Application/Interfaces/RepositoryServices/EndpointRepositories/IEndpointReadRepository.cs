using Application.Interfaces.GenericRepositoryServices;
using Domain.BaseProjeEntities.IdentityEntities;

namespace Application.Interfaces.RepositoryServices.EndpointRepositories
{
    public interface IEndpointReadRepository : IReadRepository<Endpoint, Guid>
    {
    }
}
