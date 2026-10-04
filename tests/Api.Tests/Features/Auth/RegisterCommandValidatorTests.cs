using Api.Features.Auth.Register;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;

namespace Api.Tests.Features.Auth;

public sealed class RegisterCommandValidatorTests
{
    private static readonly DateTimeOffset Now = new(2026, 1, 15, 12, 0, 0, TimeSpan.Zero);

    private readonly RegisterCommandValidator _validator = new(new FakeTimeProvider(Now));

   

    private static RegisterCommand ValidCommand() => new(
        UserName: "lisa",
        Password: "Pa$$w0rd",
        KnownAs: "Lisa",
        Gender: "female",
        DateOfBirth: new DateOnly(1995, 6, 1),
        City: "London",
        Country: "UK");

    [Fact]
    public void Valid_command_passes()
    {
        _validator.TestValidate(ValidCommand()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Turning_18_today_passes()
    {
        var command = ValidCommand() with { DateOfBirth = new DateOnly(2008, 1, 15) };

        _validator.TestValidate(command).ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void One_day_under_18_fails()
    {
        var command = ValidCommand() with { DateOfBirth = new DateOnly(2008, 1, 16) };

        _validator.TestValidate(command)
            .ShouldHaveValidationErrorFor(x => x.DateOfBirth)
            .WithErrorMessage("You must be at least 18 years old to register.");
    }

    [Theory]
    [InlineData("fale")]
    [InlineData("")]
    public void Unknown_gender_fails(string gender)
    {
        var command = ValidCommand() with { Gender = gender };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void Empty_username_fails()
    {
        var command = ValidCommand() with { UserName = "" };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Username_over_50_characters_fails()
    {
        var command = ValidCommand() with { UserName = new string('a', 51) };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    public void Empty_password_fails()
    {
        var command = ValidCommand() with { Password = "" };

        _validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Password);
    }
}