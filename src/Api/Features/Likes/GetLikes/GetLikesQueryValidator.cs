using FluentValidation;

namespace Api.Features.Likes.GetLikes;

public sealed class GetLikesQueryValidator : AbstractValidator<GetLikesQuery>
{
    public GetLikesQueryValidator()
    {
        RuleFor(x => x.Predicate)
            .Must(p => p is "liked" or "likedBy")
            .WithMessage("Predicate must be 'liked' or 'likedBy'.");

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}