using ManagamentPias.App.Common.Services;
using ManagamentPias.App.Wrappers;
using ManagamentPias.Infra.Shared.Authentication.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ManagamentPias.Infra.Shared.Services;

public class JwtTokenService : ITokenService
{
    private readonly AuthenticationSettings _authSettings;
    public JwtTokenService(AuthenticationSettings authSettings)
    {
        _authSettings = authSettings;
    }
    public TokenModel CreateAuthenticationToken(string userId, string uniqueName, IEnumerable<(string claimType, string claimValue)>? customClaims = null)
    {
        var expiration = DateTime.UtcNow.AddHours(3);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _authSettings.JwtIssuer),
                new Claim(JwtRegisteredClaimNames.Aud, _authSettings.JwtAudience),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName)
            }.Concat(
                customClaims?.Select(x => new Claim(x.claimType, x.claimValue)) ?? Enumerable.Empty<Claim>())
            ),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(_authSettings.JwtSigningKeyBase64)), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenModel(
            tokenType: "Bearer",
            accessToken: tokenString,
            expiresAt: expiration
        );
    }
}
