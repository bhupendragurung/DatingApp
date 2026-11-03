using Api.Common.Paging;
using Domain.Common;
using Infrastructure;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Members.GetMembers;

public sealed class GetMembersHandler(AppDbContext db, TimeProvider timeProvider)
    : IQueryHandler<GetMembersQuery, Result<PagedResult<MemberDto>>>
{
    private static readonly Error CurrentUserNotFound =
        Error.NotFound("Members.CurrentUserNotFound", "The current user was not found.");

    public async ValueTask<Result<PagedResult<MemberDto>>> Handle(
        GetMembersQuery query,
        CancellationToken cancellationToken)
    {
        var gender = query.Gender;

        if (gender is null)
        {
            var currentUserGender = await db.Users
                .Where(u => u.Id == query.CurrentUserId)
                .Select(u => u.Gender)
                .SingleOrDefaultAsync(cancellationToken);

            if (currentUserGender is null)
            {
                return Result<PagedResult<MemberDto>>.Failure(CurrentUserNotFound);
            }

            gender = currentUserGender == "male" ? "female" : "male";
        }

        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var youngestDateOfBirth = today.AddYears(-query.MinAge);
        var oldestDateOfBirth = today.AddYears(-query.MaxAge - 1).AddDays(1);

        var members = db.Users
            .AsNoTracking()
            .Where(u => u.Id != query.CurrentUserId)
            .Where(u => u.Gender == gender)
            .Where(u => u.DateOfBirth >= oldestDateOfBirth && u.DateOfBirth <= youngestDateOfBirth);

        members = query.OrderBy switch
        {
            "name" => members.OrderBy(u => u.KnownAs).ThenBy(u => u.Id),
            _ => members.OrderByDescending(u => u.CreatedAt).ThenBy(u => u.Id)
        };

        var page = await members
            .Select(u => new MemberDto(
                u.Id,
                u.UserName!,
                u.KnownAs,
                u.DateOfBirth.CalculateAge(today),
                u.Gender,
                u.City,
                u.Country))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<MemberDto>>.Success(page);
    }
}