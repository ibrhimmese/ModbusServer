using Application.CustomAttributes;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.AuthorizationFiters;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        //[ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
       // [AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Reading, Definition = "Get Roles", Menu = "Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return Ok(result);
        }

        [HttpGet("GetRolesSuper")]
        //[ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Reading, Definition = "Get Roles Super", Menu = "Roles")]
        public async Task<IActionResult> GetRolesSuper()
        {
            var result = await _roleService.GetAllRolesSuperAsync();
            return Ok(result);
        }

        [HttpGet("getById")]
       // [ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Reading, Definition = "Get RoleById", Menu = "Roles")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var result =await _roleService.GetRoleByIdAsync(id);
            return Ok(result);
        }

        [HttpPost()]
       // [ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
       // [AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Writing, Definition = "Create Role", Menu = "Roles")]
        public async Task<IActionResult> CreateRole(string name)
        {
            var result = await _roleService.CreateRoleAsync(name);
            return Ok(result);
        }

        [HttpPut]
        [ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Updating, Definition = "Update Role", Menu = "Roles")]
        public async Task<IActionResult> UpdateRole(Guid id, string name)
        {
            var result = await _roleService.UpdateRoleAsync(id, name);
            return Ok(result);
        }
        [HttpDelete]
        [ServiceFilter(typeof(RolePermissionFilter))]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Deleting, Definition = "Delete Role", Menu = "Roles")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return Ok(result);
        }
    }
}
