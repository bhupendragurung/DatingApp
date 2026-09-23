using Domain.Auth;
using Domain.Common;
using Infrastructure;
using Infrastructure.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;

namespace Api.Features.Auth.Register;

public sealed class RegisterHandler(UserManager<AppUser> userManager, AppDbContext db)
    : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    public async ValueTask<Result<UserDto>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await userManager.FindByNameAsync(command.UserName) is not null)
        {
            return Result<UserDto>.Failure(UsernameTaken);
        }

        var user = new AppUser
        {
            UserName = command.UserName,
            KnownAs = command.KnownAs,
            Gender = command.Gender,
            DateOfBirth = command.DateOfBirth,
            City = command.City,
            Country = command.Country
        };

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var created = await userManager.CreateAsync(user, command.Password);
        if (!created.Succeeded)
        {
            return Result<UserDto>.Failure(ToError(created.Errors));
        }

        var roleAdded = await userManager.AddToRoleAsync(user, Roles.Member);
        if (!roleAdded.Succeeded)
        {
            throw new InvalidOperationException(
                $"Could not add role '{Roles.Member}': {string.Join(", ", roleAdded.Errors.Select(e => e.Code))}");
        }

        await transaction.CommitAsync(cancellationToken);

        return Result<UserDto>.Success(new UserDto(user.Id, user.UserName!, user.KnownAs));
    }

    private static readonly Error UsernameTaken =
        Error.Conflict("Auth.UsernameTaken", "Username is already taken.");

    private static Error ToError(IEnumerable<IdentityError> identityErrors)
    {
        var errors = identityErrors.ToList();

        if (errors.Any(e => e.Code == nameof(IdentityErrorDescriber.DuplicateUserName)))
        {
            return UsernameTaken;
        }

        return Error.Validation(errors
            .GroupBy(e => e.Code.StartsWith("Password")
                ? nameof(RegisterCommand.Password)
                : nameof(RegisterCommand.UserName))
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()));
    }
}