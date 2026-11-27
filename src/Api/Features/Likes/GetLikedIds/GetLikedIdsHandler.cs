using Domain.Common;
using Infrastructure;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Likes.GetLikedIds;

public sealed class GetLikedIdsHandler(AppDbContext db)
    : IQueryHandler<GetLikedIdsQuery, Result<Guid[]>>
{
    public async ValueTask<Result<Guid[]>> Handle(
        GetLikedIdsQuery query,
        CancellationToken cancellationToken)
    {
        var ids = await db.Likes
            .AsNoTracking()
            .Where(l => l.SourceUserId == query.CurrentUserId)
            .Select(l => l.TargetUserId)
            .ToArrayAsync(cancellationToken);

        return Result<Guid[]>.Success(ids);
    }
}