using Application.JwtTokenHandlerInterface.DTOs;
using Domain.BaseProjeEntities.IdentityEntities;

namespace Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;

public interface IUserService
{
    Task<CreateUserResponseDTO> CreateAsync(CreateUserDTO model); //5   Register işlemi

    Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);  //11 refresh token yenileme

    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);

    Task<List<ListUserAndRoleDetailsDto>> GetAllUsersAsync();
    //Task<List<ListUserAndRoleDto>> GetAllUsersAndRolesAsync();
    //Task<List<ListUserAndRoleDetailsDto>> GetAllUsersAndRolesDetailAsync();
    Task<List<ListUserAndRoleDetailsDto>> GetAllUsersAndRolesDetailByAdministratorAsync(); //2  Tüm kullanıcıları ve rolleri getirir, sadece admin görebilir.

    Task<ListUserAndRoleDetailsDto>GetCurrentUserDetailAsync(string userId);   //1 Mevcut kullanıcıyı ve rollerini getirir.

    Task<string> GetByIdUserNameAsync(Guid userId);



    Task AssignRoleToUserAsync(string userId , string[] roles); //3 Kullanıcıya roller atar.

    Task<string[]> GetRolesToUserAsync(string userIdOrName);

    Task<bool> HasRolePermissionToEndpointAsync(string name, string code);  //10 Bir kullanıcının belirli bir endpointe erişim iznine sahip olup olmadığını kontrol eder.

    Task<List<AppUser>> GetUsersByIdsAsync(List<Guid> userIds);

    Task UpdateNameAsync(string nameSurname,CancellationToken cancellationToken);  //7  NameSurname güncelleme
    Task UpdateUserNameAsync(string userName, CancellationToken cancellationToken); //8 UserName güncelleme
    Task UpdateEmailAsync(string email, CancellationToken cancellationToken);  //9 Email güncelleme

    Task UpdateUserFromAdminAsync(Guid userId, string nameSurname, string userName, string email,
       string newPassword, string confirmNewPassword, CancellationToken cancellationToken);  //4 Admin tarafından kullanıcı güncelleme

    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);  //6 Kullanıcı silme

}
