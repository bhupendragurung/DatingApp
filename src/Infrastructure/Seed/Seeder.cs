using System.Text.Json;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed;

public sealed class Seeder(UserManager<AppUser> userManager, AppDbContext db, ILogger<Seeder> logger)
{
    public async Task SeedUsersAsync(string password, CancellationToken cancellationToken)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Seed", "users.json");
        await using var stream = File.OpenRead(path);
        var seedUsers = await JsonSerializer.DeserializeAsync<List<SeedUser>>(
            stream, JsonSerializerOptions.Web, cancellationToken) ?? [];

        foreach (var seed in seedUsers)
        {
            if (await userManager.FindByNameAsync(seed.UserName) is not null)
            {
                continue;
            }

            var user = new AppUser
            {
                UserName = seed.UserName,
                KnownAs = seed.KnownAs,
                Gender = seed.Gender,
                DateOfBirth = seed.DateOfBirth,
                City = seed.City,
                Country = seed.Country
            };

            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            EnsureSucceeded(await userManager.CreateAsync(user, password), seed.UserName);
            EnsureSucceeded(await userManager.AddToRolesAsync(user, seed.Roles), seed.UserName);

            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Seeded user {UserName} with roles {Roles}", seed.UserName, seed.Roles);
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string userName)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Seeding '{userName}' failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}