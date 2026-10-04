using System.Text;
using Api.Common.Auth;
using Infrastructure.Identity;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Api.Tests.Common.Auth;

public sealed class TokenServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private static readonly JwtOptions Jwt = new()
    {
        Key = new string('k', 64),
        Issuer = "test-issuer",
        Audience = "test-audience",
        ExpiryMinutes = 60
    };

    private readonly TokenService _service =
        new(Microsoft.Extensions.Options.Options.Create(Jwt), new FakeTimeProvider(Now));

    private static AppUser NewUser() => new()
    {
        Id = Guid.NewGuid(),
        UserName = "lisa",
        KnownAs = "Lisa",
        Gender = "female",
        DateOfBirth = new DateOnly(1995, 6, 1),
        City = "London",
        Country = "UK"
    };

    private static TokenValidationParameters ValidationParameters(string key) => new()
    {
        ValidIssuer = Jwt.Issuer,
        ValidAudience = Jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateLifetime = false // issued at a fake time; expiry is checked separately
    };

    [Fact]
    public async Task Token_is_valid_for_configured_key_issuer_and_audience()
    {
        var token = _service.CreateToken(NewUser(), ["Member"]);

        var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, ValidationParameters(Jwt.Key));

        Assert.True(result.IsValid, result.Exception?.Message);
    }

    [Fact]
    public async Task Token_is_rejected_with_a_different_key()
    {
        var token = _service.CreateToken(NewUser(), ["Member"]);

        var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, ValidationParameters(new string('x', 64)));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Token_contains_user_id_username_and_roles()
    {
        var user = NewUser();

        var jwt = new JsonWebToken(_service.CreateToken(user, ["Member", "Admin"]));

        Assert.Equal(user.Id.ToString(), jwt.Subject);
        Assert.Equal("lisa", jwt.GetClaim(JwtRegisteredClaimNames.UniqueName).Value);
        Assert.Equal(new[] { "Member", "Admin" }, jwt.Claims.Where(c => c.Type == "role").Select(c => c.Value));
    }

    [Fact]
    public void Token_expires_after_configured_minutes()
    {
        var jwt = new JsonWebToken(_service.CreateToken(NewUser(), ["Member"]));

        Assert.Equal(Now.UtcDateTime, jwt.IssuedAt);
        Assert.Equal(Now.UtcDateTime.AddMinutes(Jwt.ExpiryMinutes), jwt.ValidTo);
    }
}