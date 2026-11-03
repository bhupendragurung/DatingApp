using FluentValidation;

namespace Api.Features.Members.GetMembers;

public sealed class GetMembersQueryValidator : AbstractValidator<GetMembersQuery>
{
    public GetMembersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);

        RuleFor(x => x.Gender)
            .Must(g => g is null or "male" or "female")
            .WithMessage("Gender must be either 'male' or 'female'.");

        RuleFor(x => x.MinAge).GreaterThanOrEqualTo(18);

        RuleFor(x => x.MaxAge)
            .LessThanOrEqualTo(120)
            .GreaterThanOrEqualTo(x => x.MinAge)
            .WithMessage("MaxAge must be between MinAge and 120.");

        RuleFor(x => x.OrderBy)
            .Must(o => o is "newest" or "name")
            .WithMessage("OrderBy must be 'newest' or 'name'.");
    }
}