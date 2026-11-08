using Domain.Common;
using Infrastructure;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Members.UpdateProfile;

public sealed class UpdateProfileHandler(AppDbContext db)
    : ICommandHandler<UpdateProfileCommand, Result>
{
    private static readonly Error MemberNotFound =
        Error.NotFound("Members.NotFound", "The member was not found.");

    public async ValueTask<Result> Handle(
        UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await db.Users
            .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(MemberNotFound);
        }

        user.City = command.City.Trim();
        user.Country = command.Country.Trim();
        user.Introduction = Clean(command.Introduction);
        user.LookingFor = Clean(command.LookingFor);
        user.Interests = Clean(command.Interests);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}