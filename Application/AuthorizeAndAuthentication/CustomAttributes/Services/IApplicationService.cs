using Application.CustomAttributes.DTOs;

namespace Application.CustomAttributes.Services;

public interface IApplicationService
{
    List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
}
