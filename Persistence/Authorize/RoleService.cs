using Application.ExceptionTypes;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.DTOs;
using Domain.BaseProjeEntities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Authorize;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRoleAsync(string name)
    {
       IdentityResult result = await _roleManager.CreateAsync(new() { Name = name });
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        AppRole role =await _roleManager.FindByIdAsync(id.ToString());
      IdentityResult result =  await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<List<AppRoleDto>> GetAllRolesAsync()
    {
        return await _roleManager.Roles
            .Select(role => new AppRoleDto
            {
                Id = role.Id,
                Name = role.Name
            })
            .ToListAsync();
    }

    public async Task<List<AppRoleDto>> GetAllRolesSuperAsync()
    {
        return await _roleManager.Roles.Where(role=>role.Name != "Super")
            .Select(role => new AppRoleDto
            {
                Id = role.Id,
                Name = role.Name
            })
            .ToListAsync();
    }

    public async Task<AppRoleDto> GetRoleByIdAsync(Guid id)
    {
        
        var role = await _roleManager.Roles
            .Where(r => r.Id == id)
            .Select(r => new AppRoleDto
            {
                Id = r.Id,
                Name = r.Name
            })
            .FirstOrDefaultAsync();

        if (role is null)
        {
            throw new NotFoundException($"Role with ID {id} not found.");
        }

        return role;
    }


    public async Task<bool> UpdateRoleAsync(Guid id,string name)
    {
        AppRole role = await _roleManager.FindByIdAsync(id.ToString());
        role.Name = name;
        IdentityResult result = await _roleManager.UpdateAsync(role);

        return result.Succeeded;
    }
}
