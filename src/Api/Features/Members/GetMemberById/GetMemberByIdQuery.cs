using Domain.Common;
using Mediator;

namespace Api.Features.Members.GetMemberById;

public sealed record GetMemberByIdQuery( Guid Id): IQuery<Result<MemberDetailDto>>;
