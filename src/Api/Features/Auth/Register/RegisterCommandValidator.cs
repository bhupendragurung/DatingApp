using FluentValidation;

namespace Api.Features.Auth.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.UserName)
    .NotEmpty().WithMessage("UserName is required.")
    .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");
        RuleFor(x=>x.Password).NotEmpty();
        RuleFor(x=>x.KnownAs).NotEmpty().MaximumLength(50).WithMessage("KnownAs is required and should not exceed 50 characters.");
        RuleFor(x=>x.Gender).Must(g=>g is "male" or "female").WithMessage("Gender must be either 'male' or 'female'.");
        RuleFor(x=>x.City).NotEmpty().MaximumLength(100).WithMessage("City is required and should not exceed 100 characters.");
        RuleFor(x=>x.Country).NotEmpty().MaximumLength(100).WithMessage("Country is required and should not exceed 100 characters.");
        RuleFor(x=>x.DateOfBirth)
        .NotEmpty().WithMessage("Date of birth is required.")
        .LessThanOrEqualTo(_ => DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime).AddYears(-18))
    .WithMessage("You must be at least 18 years old to register.");
    }
}