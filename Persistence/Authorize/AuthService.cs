using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Application.ExceptionTypes;
using Application.Interfaces.MailService;
using Application.JwtTokenHandlerInterface;
using Application.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Domain.BaseProjeEntities.IdentityEntities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using System.Text;

namespace Persistence.Authorize;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IMailService _mailService;
    private readonly IUserIdService _userIdService;

    public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, IUserService userService, IMailService mailService, IUserIdService userIdService)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _configuration = configuration;
        _userService = userService;
        _mailService = mailService;
        _userIdService = userIdService;
    }



    public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
    {
        AppUser? user = await _userManager.FindByNameAsync(usernameOrEmail);
        if (user is null)
            user = await _userManager.FindByEmailAsync(usernameOrEmail);
        if (user is null)
        {
            throw new NotFoundException("Username or password is incorrect! Please try again.");
        }

        var result = await _userManager.CheckPasswordAsync(user, password);

       // SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (result)  //Authentication başarılı
        {

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 1);
            return token;
        }

        throw new AuthorizationException("Password is incorrect!");
    }


   

    public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
    {

        var clientId = _configuration["GoogleAuth:Client_ID"];
        if (string.IsNullOrEmpty(clientId))
        {
            throw new OperationException("Google Client ID is not configured.");
        }

        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string> { clientId }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

        AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user is null)
        {
            throw new NotFoundException("Kullanıcı Bulunamadı");
        }
        else
        return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);

    }

    

    async Task<Token> CreateUserExternalAsync(AppUser? user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
    {
        bool result = user != null;

        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser()
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    UserName = email,
                    NameSurname = name,
                };
                var identityResult = await _userManager.CreateAsync(user);
                result = identityResult.Succeeded;
            }
        }
        if (result)
        {
            await _userManager.AddLoginAsync(user, info);

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);

            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 1);

            return token;
        }

        throw new System.Security.Authentication.AuthenticationException("Invalid external authentiation");

    }


    public Task FacebookLoginAsync()
    {
        throw new NotImplementedException();
    }


    public Task TwitterLoginAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user is not null && user?.RefreshTokenEndDate > DateTime.UtcNow)
        {
            Token token = _tokenHandler.CreateAccessToken(60,user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 30);
            return token;
        }
        else
        {
            throw new NotFoundException();
        }
    }

    public async Task PasswordResetAsync(string email)
    {
       AppUser? user = await _userManager.FindByEmailAsync(email);
        if(user is not null)
        {
           string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
           resetToken = WebEncoders.Base64UrlEncode(tokenBytes);

           await _mailService.SendPasswordResetMailAsync(email,user.Id.ToString(),resetToken);
        }
    }

    public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
    {
       AppUser? user= await _userManager.FindByIdAsync(userId);
        if(user is not null )
        {
            byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
            Encoding.UTF8.GetString(tokenBytes);

           return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
        }
        return false;
    }

    public async Task ChangePasswordAsync(string currentPassword, string confirmPassword, string newPassword, CancellationToken cancellationToken)
    {
        var userId = await _userIdService.GetUserIdAsync(cancellationToken);

        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("The User was not found.");
        }

        // Yeni şifre doğrulaması
        if (!newPassword.Equals(confirmPassword))
        {
            throw new BusinessException("New passwords do not match!");
        }

        // Şifre değiştirme işlemi
        IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BusinessException($"Failed to change password: {errors}");
        }

        // Güvenlik damgasını güncelle
        await _userManager.UpdateSecurityStampAsync(user);
    }
}
