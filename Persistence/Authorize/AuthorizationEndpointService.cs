using Application.CustomAttributes.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.BaseProjeEntities.IdentityEntities;
using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Application.Interfaces.RepositoryServices.MenuRepositories;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;

namespace Persistence.Authorize;

public class AuthorizationEndpointService : IAuthorizationEndpointService
{
     private readonly IApplicationService _applicationService;
     private readonly IEndpointReadRepository _endpointReadRepository;
     private readonly IEndpointWriteRepository _endpointWriteRepository;
     private readonly IMenuReadRepository _menuReadRepository;
     private readonly IMenuWriteRepository _menuWriteRepository;
     private readonly RoleManager<AppRole> _roleManager;

    public AuthorizationEndpointService(IApplicationService applicationService, IEndpointReadRepository endpointReadRepository, IEndpointWriteRepository endpointWriteRepository, IMenuReadRepository menuReadRepository, IMenuWriteRepository menuWriteRepository, RoleManager<AppRole> roleManager)
    {
        _applicationService = applicationService;
        _endpointReadRepository = endpointReadRepository;
        _endpointWriteRepository = endpointWriteRepository;
        _menuReadRepository = menuReadRepository;
        _menuWriteRepository = menuWriteRepository;
        _roleManager = roleManager;
    }

    public async Task AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
    {
        // Menü verisini al veya oluştur
        Menu? _menu = await _menuReadRepository.GetAsync(m => m.Name == menu);
        if (_menu is null)
        {
            _menu = new Menu
            {
                Name = menu,
            };
            await _menuWriteRepository.AddAsync(_menu);
        }

        // Endpoint verisini al veya oluştur
        Endpoint? endpoint = await _endpointReadRepository.GetAsync(e => e.Code == code && e.Menu.Name == menu, include: m => m.Include(m => m.Menu).Include(r => r.Roles));
        if (endpoint is null)
        {
            var action = _applicationService.GetAuthorizeDefinitionEndpoints(type).FirstOrDefault(m => m.Name == menu)?
                .Actions.FirstOrDefault(e => e.Code == code);

            if (action == null)
            {
                // Null ise uygun bir hata mesajı döndür veya varsayılan değerler kullan
                throw new ArgumentNullException(nameof(action), "Action cannot be null.");
            }

            endpoint = new Endpoint
            {
                Id = Guid.NewGuid(),
                Code = action.Code,
                ActionType = action.ActionType,
                HttpType = action.HttpType,
                Definition = action.Definition,
                Menu = _menu,
            };

            await _endpointWriteRepository.AddAsync(endpoint);
        }

        // Koleksiyonun kopyasını oluştur
        var rolesToRemove = endpoint.Roles.ToList();

        // Mevcut rolleri kaldır
        foreach (var role in rolesToRemove)
        {
            endpoint.Roles.Remove(role);
        }

        // Yeni rolleri ekle
        var appRoles = await _roleManager.Roles.Where(r => roles.Contains(r.Name)).ToListAsync();
        foreach (var role in appRoles)
        {
            endpoint.Roles.Add(role);
        }

        // Değişiklikleri kaydet
        await _endpointWriteRepository.SaveChangesAsync();
    }


    public async Task<List<string>> GetRolesToEndpointAsync(string code, string menu)
    {
      Endpoint? endpoint = await _endpointReadRepository.GetAsync(e => e.Code == code && e.Menu.Name == menu, include:r=>r.Include(r=>r.Roles).Include(m=>m.Menu));
        if(endpoint is not null)
       return endpoint.Roles.Select(r => r.Name).ToList();
        return null;
    }
}
