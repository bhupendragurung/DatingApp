using FluentValidation;

namespace Api.Features.Likes.LikeUser;

public sealed class LikeUserCommandValidator : AbstractValidator<LikeUserCommand>
{
    public LikeUserCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Target user ID is required.")
            .NotEqual(x => x.UserId).WithMessage("You cannot like yourself.");
    }
}