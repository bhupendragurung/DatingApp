using Domain.Common;
using Mediator;

namespace Api.Features.Likes.GetLikedIds;

public sealed record GetLikedIdsQuery(Guid CurrentUserId) : IQuery<Result<Guid[]>>;