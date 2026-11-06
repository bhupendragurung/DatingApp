
using Domain.Common;
using Infrastructure;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Members.GetMemberById;

public sealed class GetMemberByIdHandler(AppDbContext db, TimeProvider timeProvider)
    : IQueryHandler<GetMemberByIdQuery, Result<MemberDetailDto>>
{
    private static readonly Error MemberNotFound =
        Error.NotFound("Members.NotFound", "The member was not found.");

    public async ValueTask<Result<MemberDetailDto>> Handle(
        GetMemberByIdQuery query,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);

        var member = await db.Users
            .AsNoTracking()
            .Where(u => u.Id == query.Id)
            .Select(u => new MemberDetailDto(
                u.Id,
                u.UserName!,
                u.KnownAs,
                u.DateOfBirth.CalculateAge(today),
                u.Gender,
                u.City,
                u.Country,
                u.CreatedAt,
                u.Introduction,
                u.LookingFor,
                u.Interests))
            .SingleOrDefaultAsync(cancellationToken);

        return member is null
            ? Result<MemberDetailDto>.Failure(MemberNotFound)
            : Result<MemberDetailDto>.Success(member);
    }
}