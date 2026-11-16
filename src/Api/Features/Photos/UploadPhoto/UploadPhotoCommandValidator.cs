using FluentValidation;

namespace Api.Features.Photos.UploadPhoto;

public sealed class UploadPhotoCommandValidator : AbstractValidator<UploadPhotoCommand>
{
    public const long MaxFileSize = 5 * 1024 * 1024;

    public static readonly string[] AllowedContentTypes =
        ["image/jpeg", "image/png", "image/webp"];

    public UploadPhotoCommandValidator()
    {
        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("The file is empty.")
            .LessThanOrEqualTo(MaxFileSize).WithMessage("The file must be 5 MB or smaller.");

        RuleFor(x => x.ContentType)
            .Must(type => AllowedContentTypes.Contains(type))
            .WithMessage("Only JPEG, PNG and WebP images are allowed.");
    }
}