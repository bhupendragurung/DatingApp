using FluentValidation;

namespace Api.Features.Photos.DeletePhoto;

public sealed class DeletePhotoCommandValidator : AbstractValidator<DeletePhotoCommand>
{
    public DeletePhotoCommandValidator()
    {
        RuleFor(x => x.PhotoId)
            .NotEmpty().WithMessage("Photo ID is required.");
    }
}