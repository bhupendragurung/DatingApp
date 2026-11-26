using Api.Common.Paging;
using Api.Features.Members;
using Domain.Common;
using Infrastructure;
using Infrastructure.Identity;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Likes.GetLikes;

public sealed class GetLikesHandler(AppDbContext db, TimeProvider timeProvider)
    : IQueryHandler<GetLikesQuery, Result<PagedResult<MemberDto>>>
{
    public async ValueTask<Result<PagedResult<MemberDto>>> Handle(
        GetLikesQuery query,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

          var likes = db.Likes.AsNoTracking();

        IQueryable<AppUser> users = query.Predicate == "likedBy"
            ? likes
                .Where(l => l.TargetUserId == query.CurrentUserId)
                .OrderByDescending(l => l.CreatedAt)
                .ThenBy(l => l.SourceUserId)
                .Select(l => l.SourceUser)
            : likes
                .Where(l => l.SourceUserId == query.CurrentUserId)
                .OrderByDescending(l => l.CreatedAt)
                .ThenBy(l => l.TargetUserId)
                .Select(l => l.TargetUser);

        var page = await users
            .Select(u => new MemberDto(
                u.Id,
                u.UserName!,
                u.KnownAs,
                u.DateOfBirth.CalculateAge(today),
                u.Gender,
                u.City,
                u.Country,
                u.LastActive,
                u.Photos
                    .Where(p => p.IsMain)
                    .Select(p => p.Url)
                    .FirstOrDefault()))
            .ToPagedResultAsync(query.Page, query.PageSize, cancellationToken);

        return Result<PagedResult<MemberDto>>.Success(page);
    }
}