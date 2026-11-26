using Api.Common.Paging;
using Api.Features.Members;
using Domain.Common;
using Mediator;

namespace Api.Features.Likes.GetLikes;

public sealed record GetLikesQuery : IQuery<Result<PagedResult<MemberDto>>>
{
    public Guid CurrentUserId { get; init; }
    public string Predicate { get; init; } = "liked";
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}