using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Common.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new InvalidOperationException("The token has no 'sub' claim.");

        return Guid.Parse(value);
    }
}