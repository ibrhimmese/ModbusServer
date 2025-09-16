using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.ExceptionTypes;
using Application.Interfaces.RepositoryServices.EndpointRepositories;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.JwtTokenHandlerInterface.DTOs;
using Domain.BaseProjeEntities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Persistence.Authorize;

public class UserService(UserManager<AppUser> userManager,IEndpointReadRepository endpointReadRepository
    ,IUserIdService userIdService) : IUserService
{
    public async Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO model)
    {


        IdentityResult result = await userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid(),
            UserName = model.Username,
            Email = model.Email,
            NameSurname = model.NameSurname,
        }, model.Password);


        CreateUserResponseDTO response = new() { Succeeded = result.Succeeded };


        if (result.Succeeded)
        {
            response.Message = "Kullanıcı başarıyla Oluşturulmuştur";
        }
        else
        {
            foreach (var error in result.Errors)
            {
                response.Message += $"{error.Code} - {error.Description}\n";
            }
        }
        return response;
    }

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
    {

        if (user is not null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddMinutes(addOnAccessTokenDate);
            await userManager.UpdateAsync(user);
        }
        else
        {
            throw new NotFoundException();
        }

    }
    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser? user = await userManager.FindByIdAsync(userId);
        if(user is not null)
        {
            byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
            resetToken = Encoding.UTF8.GetString(tokenBytes);
          IdentityResult result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if(result.Succeeded)
            {
               await userManager.UpdateSecurityStampAsync(user);
            }
            else
            {
                throw new OperationException("Şifre güncelleme işlemi başarısız");
            }
        }
    }

    public async Task<List<ListUserAndRoleDetailsDto>> GetAllUsersAsync()
    {
        var allUsers = await userManager.Users.ToListAsync();
        var superAdminUsers = await userManager.GetUsersInRoleAsync("Super");

        // Get the IDs of users who have the "Super" role
        var superAdminUserIds = new HashSet<Guid>(superAdminUsers.Select(u => u.Id));

        var result = new List<ListUserAndRoleDetailsDto>();

        // Only include users who are NOT in the "Super" role
        foreach (var user in allUsers.Where(u => !superAdminUserIds.Contains(u.Id)))
        {
            var roles = await userManager.GetRolesAsync(user);

            var userWithRoles = new ListUserAndRoleDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                UserName = user.UserName,
                Roles = roles.Select(role => new AppRoleNameDto { Name = role }).ToList()
            };

            result.Add(userWithRoles);
        }

        return result;
    }

    //GetAllUsersAndRoles 
    public async Task<List<ListUserAndRoleDetailsDto>> GetAllUsersAndRolesDetailByAdministratorAsync()
    {
       
        var users = await userManager.Users.ToListAsync();

        var result = new List<ListUserAndRoleDetailsDto>();

        foreach (var user in users)
        {
            // First, await GetRolesAsync outside of the Select
            var roles = await userManager.GetRolesAsync(user);

            // Now, map the roles to AppRoleDto
            var userWithRoles = new ListUserAndRoleDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                UserName = user.UserName,
               
                Roles = roles.Select(role => new AppRoleNameDto { Name = role }).ToList()
            };

            result.Add(userWithRoles);
        }

        return result;
    }


    public async Task<ListUserAndRoleDetailsDto> GetCurrentUserDetailAsync(string userId)
    {
        AppUser? user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        }
        var roles = await userManager.GetRolesAsync(user);
        return new ListUserAndRoleDetailsDto
        {
            Id = user.Id,
            Email = user.Email,
            NameSurname = user.NameSurname,
            UserName = user.UserName,
            
            Roles = roles.Select(role => new AppRoleNameDto { Name = role }).ToList()
        };
    }

    public async Task AssignRoleToUserAsync(string userId, string[] roles)
    {
       AppUser user = await userManager.FindByIdAsync(userId);
        if(user is not null )
        {
           var userRoles =await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            await userManager.AddToRolesAsync(user, roles);
        }
    }

    public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
    {
        AppUser user = await userManager.FindByNameAsync(userIdOrName);

        
        if(user is not null )
        {
            var userRoles = await userManager.GetRolesAsync(user);
           return userRoles.ToArray();
        }
        else
        {
            throw new NotFoundException("User Bulunamadı");
        }
    }

    public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
    {
        var userRoles = await GetRolesToUserAsync(name);
        if (!userRoles.Any())
            return false;

      Endpoint? endpoint = await endpointReadRepository.GetAsync(e => e.Code == code, include: r => r.Include(r => r.Roles));
        if(endpoint is null)
        {
            return false;
        }
        var hasRole = false;

        var endpointRoles = endpoint.Roles.Select(r => r.Name);

        foreach (var userRole in userRoles)
        {

            foreach (var endpointRole in endpointRoles)
                if (userRole == endpointRole)
                    return true;
        }
        return false;
    }

    public async Task<List<AppUser>> GetUsersByIdsAsync(List<Guid> userIds)
    {
        var users = new List<AppUser>();

        foreach (var userId in userIds)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception($"User with ID {userId} not found.");
            users.Add(user);
        }

        return users;
    }

    public async Task UpdateNameAsync(string nameSurname, CancellationToken cancellationToken)
    {
        var userId = await userIdService.GetUserIdAsync();
        AppUser? user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        }
        if (string.IsNullOrWhiteSpace(nameSurname))
        {
            throw new BusinessException("Ad Soyad alanı adı boş olamaz.");
        }

        user.NameSurname = nameSurname;
        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Ad güncelleme işlemi başarısız: {errors}");
        }
        
    }

    public async Task UpdateUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        var userId = await userIdService.GetUserIdAsync();
        AppUser? user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        }
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new BusinessException("Kullanıcı adı boş olamaz.");
        }

        user.UserName = userName;

        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Kullanıcı adı güncelleme işlemi başarısız: {errors}");
        }
    }

    public async Task UpdateEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userId = await userIdService.GetUserIdAsync();
        AppUser? user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");
        }
     
       
        user.Email = email;
        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Email güncelleme işlemi başarısız: {errors}");
        }
    }

    //Admin olarak (en üst yetkilinin update yapması)
    public async Task UpdateUserFromAdminAsync(Guid userId, string nameSurname, string userName, string email,
        string newPassword, string confirmNewPassword, CancellationToken cancellationToken)
    {
        // Kullanıcıyı bul
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new BusinessException("Kullanıcı bulunamadı.");
        }

        // E-posta formatını kontrol et
        if (!IsValidEmail(email))
        {
            throw new BusinessException("Geçersiz e-posta formatı.");
        }

        // Yeni şifre doğrulaması
        if (!newPassword.Equals(confirmNewPassword))
        {
            throw new BusinessException("Yeni şifreler eşleşmiyor.");
        }

        // Şifre değiştirme işlemi
        if (!string.IsNullOrEmpty(newPassword))
        {
            var result = await userManager.RemovePasswordAsync(user); // Mevcut şifreyi kaldır
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Şifreyi kaldırma işlemi başarısız: {errors}");
            }

            result = await userManager.AddPasswordAsync(user, newPassword); // Yeni şifreyi ekle
            if (!result.Succeeded)
            {
                string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Yeni şifreyi ekleme işlemi başarısız: {errors}");
            }
        }

        // Kullanıcı adını ve e-posta adresini güncelle
        user.UserName = userName;
        user.Email = email;
        user.NameSurname = nameSurname; // Eğer böyle bir alan varsa.

        // Kullanıcıyı güncelle
        IdentityResult updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            string errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new BusinessException($"Kullanıcı bilgileri güncellenemedi: {errors}");
        }
    }

    // E-posta formatını kontrol eden yardımcı metot
    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Kullanıcıyı bul
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new BusinessException("Kullanıcı bulunamadı.");
        }

        // Kullanıcıyı sil
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Kullanıcı silme işlemi başarısız: {errors}");
        }
    }

    public async Task<string> GetByIdUserNameAsync(Guid userId)
    {
        AppUser? user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new BusinessException("Kullanıcı bulunamadı.");
        }
        return user.NameSurname;
    }

}
