using Application.Interfaces.GenericRepositoryServices;
using Domain.BaseProjeEntities.IdentityEntities;

namespace Application.Interfaces.RepositoryServices.MenuRepositories;

public interface IMenuWriteRepository : IWriteRepository<Menu, Guid>
{

}