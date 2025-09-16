using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.CustomAttributes;
using Application.Features.Auth.ChangePassword;
using Application.Features.Auth.CreateAssignRoleToUser;
using Application.Features.Auth.DeleteUser;
using Application.Features.Auth.GetAllUserAndRole;
using Application.Features.Auth.GetByIdUserName;
using Application.Features.Auth.GetCurrentUser;
using Application.Features.Auth.GetUsers;
using Application.Features.Auth.Login;
using Application.Features.Auth.RefreshToken;
using Application.Features.Auth.Register;
using Application.Features.Auth.Register.CreateUserByAdministrator;
using Application.Features.Auth.UpdateUserFromAdmin;
using Application.Features.Auth.UpdateUsers.UpdateEmail;
using Application.Features.Auth.UpdateUsers.UpdateName;
using Application.Features.Auth.UpdateUsers.UpdateUserName;
using Application.Interfaces.MailService;
using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Domain.BaseProjeEntities.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerAPI.AuthorizationFiters;
using System.Threading;

namespace ServerAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
      
        private readonly IEndpointReadRepository _endpointReadRepository;
        private readonly IMailService _mailService;
        private readonly IUserIdService _userIdService;

        public UsersController(IMailService mailService, IEndpointReadRepository endpointReadRepository, IUserIdService userIdService)
        {
            _mailService = mailService;
            _endpointReadRepository = endpointReadRepository;
            _userIdService = userIdService;
        }

        



        [HttpPost("CreateNoTenant")]
        //[ServiceFilter(typeof(RolePermissionFilter))]
        //[Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Reading, Definition = "Create User", Menu = "Users")]
        public async Task<IActionResult> Create(CreateUserByNoTenantIdAdmin createUserByNoTenantIdAdmin)
        {
            var response = await Mediator.Send(createUserByNoTenantIdAdmin);
            return Ok(response);
        }

     

        [HttpPost("[action]")]

        public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
        {
            var response = await Mediator.Send(loginUserCommand);
            return Ok(response);
        }

        [HttpPost("[action]")]
        
        public async Task<IActionResult> RefreshToken(RefreshTokenLoginCommand loginUserCommand)
        {
            var response = await Mediator.Send(loginUserCommand);
            return Ok(response);
        }






        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        //[AuthorizeDefinition(ActionType = Application.CustomAttributes.Enums.ActionType.Writing, Definition = "Assign Role To User", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommand assignRoleToUser)
        {
            var response = await Mediator.Send(assignRoleToUser);
            return Ok(response);
        }


        [HttpGet("GetAllNoSuperAdminUsers")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetAllNoSuperAdminUsers(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new UserListQueryCommand(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetAllUserAndRoleDetailsByAdministrator")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetAllUserAndRoleDetailsByAdministrator(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetAllUserAndRolesDetailByAdministratorQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetCurrentUser")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetCurrentUserQuery(), cancellationToken);
            return Ok(result);
        }


        [HttpPut("ChangePassword")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPut("UpdateName")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateUserNameCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPut("UpdateUserName")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserUserNameCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPut("UpdateEmail")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserEmailCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("UpdateUserFromAdmin")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateUserFromAdmin([FromBody] UpdateUserFromAdminCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpDelete("DeleteUser")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserCommand command, CancellationToken cancellationToken)
        {
            bool result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetUserIdByNameSurname")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetById([FromQuery] GetByIdUserNameCommand getByIdBuildingQuery, CancellationToken cancellationToken)
        {
            GetByIdUserNameCommandResponse result = await Mediator.Send(getByIdBuildingQuery, cancellationToken);
            return Ok(result);
        }
    }

}
