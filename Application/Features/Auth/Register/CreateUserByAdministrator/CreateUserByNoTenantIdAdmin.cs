using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.DTOs;
using Application.Pipelines;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Register.CreateUserByAdministrator;

public class CreateUserByNoTenantIdAdmin : IRequest<CreatedUserResponse>
{
    public string NameSurname { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirm { get; set; }

    public CreateUserByNoTenantIdAdmin(string nameSurname, string username, string email, string password, string passwordConfirm)
    {
        NameSurname = nameSurname;
        Username = username;
        Email = email;
        Password = password;
        PasswordConfirm = passwordConfirm;
    }
}


public class CreateUserByNoTenantIdAdminHandler : IRequestHandler<CreateUserByNoTenantIdAdmin, CreatedUserResponse>
{
    private readonly IUserService _userService;
    public CreateUserByNoTenantIdAdminHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<CreatedUserResponse> Handle(CreateUserByNoTenantIdAdmin request, CancellationToken cancellationToken)
    {
        CreateUserResponseDTO response = await _userService.CreateAsync(new()
        {
            Email = request.Email,
            NameSurname = request.NameSurname,
            Password = request.Password,
            PasswordConfirm = request.PasswordConfirm,
            Username = request.Username,
            
        });
        return new CreatedUserResponse()
        {
            Message = response.Message,
            Succeeded = response.Succeeded
        };
    }
}