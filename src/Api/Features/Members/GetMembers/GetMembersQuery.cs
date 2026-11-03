
using Api.Common.Paging;
using Domain.Common;
using Mediator;

namespace Api.Features.Members.GetMembers;

public sealed record GetMembersQuery : IQuery<Result<PagedResult<MemberDto>>>
{
    public Guid CurrentUserId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Gender { get; init; }
    public int MinAge { get; init; } = 18;
    public int MaxAge { get; init; } = 100;
    public string OrderBy { get; init; } = "newest";
}