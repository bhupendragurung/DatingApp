using FluentValidation;

namespace Api.Features.Photos.SetMainPhoto;

public sealed class SetMainPhotoCommandValidator : AbstractValidator<SetMainPhotoCommand>
{
    public SetMainPhotoCommandValidator()
    {
        RuleFor(x => x.PhotoId)
            .NotEmpty().WithMessage("Photo ID is required.");
    }
}