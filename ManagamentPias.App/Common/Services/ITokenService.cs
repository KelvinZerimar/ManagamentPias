using ManagamentPias.App.Wrappers;

namespace ManagamentPias.App.Common.Services;

public interface ITokenService
{
    TokenModel CreateAuthenticationToken(string userId, string uniqueName, IEnumerable<(string claimType, string claimValue)>? customClaims = null);
}

