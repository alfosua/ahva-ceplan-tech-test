using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ahva.Ceplan.Contracts.Auth;
using Ahva.Ceplan.Contracts.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ahva.Ceplan.WebApi.Auth;

public class JwtTokenService(IOptions<JwtOptions> options)
{
    public TokenOutput IssueToken(UserOutput user)
    {
        var jwt = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(jwt.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.FullName ?? user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new TokenOutput
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            TokenType = "Bearer",
            ExpiresIn = (int)TimeSpan.FromMinutes(jwt.ExpirationMinutes).TotalSeconds,
        };
    }
}
