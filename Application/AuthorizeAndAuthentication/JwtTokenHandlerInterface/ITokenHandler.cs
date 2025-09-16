using Domain.BaseProjeEntities.IdentityEntities;


namespace Application.JwtTokenHandlerInterface;

public interface ITokenHandler
{
   Token CreateAccessToken(int minute, AppUser appUser);
    string CreateRefreshToken();
}
