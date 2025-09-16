using Application.JwtTokenHandlerInterface;
using Domain.BaseProjeEntities.IdentityEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.JwtTokenHandlerConcretes;

public class TokenHandler(IConfiguration configuration) : ITokenHandler
{
    public Token CreateAccessToken(int minute, AppUser user)
    {
        Token token = new();


        string? securityKeyString = configuration["Token:SecurityKey"];
        if (string.IsNullOrEmpty(securityKeyString))
        {
            throw new Exception("SecurityKey is not configured.");
        }

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(securityKeyString));

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        token.Expiration = DateTime.UtcNow.AddMinutes(minute);

        JwtSecurityToken securityToken = new(
            audience: configuration["Token:Audience"],
            issuer: configuration["Token:Issuer"],
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials,
            claims:new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()),
             new(ClaimTypes.Name, user.UserName!),
            }
            );
        //todo : user roles will be added to claims
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);

        token.RefreshToken = CreateRefreshToken();

        return token;

    }

    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }
}
