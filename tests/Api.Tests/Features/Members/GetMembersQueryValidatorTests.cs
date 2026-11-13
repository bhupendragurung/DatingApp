using Api.Features.Members.GetMembers;
using FluentValidation.TestHelper;

namespace Api.Tests.Features.Members;
public sealed class GetMembersQueryValidatorTests
{
    private readonly GetMembersQueryValidator _validator = new();


[Fact]
public void Validate_Default_Query_Should_Not_Have_Any_Validation_Errors()
    {
         var query = new GetMembersQuery();

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
     [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_ShouldHaveError_WhenPageIsLessThan1(int page)
    {
        var query = new GetMembersQuery { Page = page };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    public void Validate_ShouldNotHaveError_WhenPageSizeIsBetween1And50(int pageSize)
    {
        var query = new GetMembersQuery{PageSize = pageSize};
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Validate_ShouldHaveError_WhenPageSizeIsOutside1To50(int pageSize)
    {
        var query = new GetMembersQuery { PageSize = pageSize };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
    [Theory]
    [InlineData(null)]
    [InlineData("male")]
    [InlineData("female")]
    public void Validate_ShouldNotHaveError_WhenGenderIsAllowed(string? gender)
    {
        var query =new GetMembersQuery { Gender = gender };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Gender);
    }
    [Theory]
    [InlineData("fale")]
    [InlineData("")]
    [InlineData("Male")]
    public void Validate_ShouldHaveError_WhenGenderIsUnknown(string gender)
    {
        var query = new GetMembersQuery { Gender = gender };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Gender)
            .WithErrorMessage("Gender must be either 'male' or 'female'.");
    }
    [Fact]
    public void Validate_ShouldNotHaveError_WhenMinAgeIs18()
    {
        var query = new GetMembersQuery { MinAge = 18 };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.MinAge);
    }
    [Fact]
    public void Validate_ShouldHaveError_WhenMinAgeIsUnder18()
    {
        var query = new GetMembersQuery { MinAge = 17 };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.MinAge);
    }
     [Theory]
    [InlineData(18, 120)]
    [InlineData(30, 30)]
    public void Validate_ShouldNotHaveError_WhenMaxAgeIsBetweenMinAgeAnd120(int minAge, int maxAge)
    {
        var query = new GetMembersQuery { MinAge = minAge, MaxAge = maxAge };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.MaxAge);
    }

    [Theory]
    [InlineData(18, 121)]
    [InlineData(30, 29)]
    public void Validate_ShouldHaveError_WhenMaxAgeIsOutsideMinAgeTo120(int minAge, int maxAge)
    {
        var query = new GetMembersQuery { MinAge = minAge, MaxAge = maxAge };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.MaxAge);
    }
    [Theory]
    [InlineData("newest")]
    [InlineData("name")]
    [InlineData("lastActive")]
    public void Validate_ShouldNotHaveError_WhenOrderByIsAllowed(string orderBy)
    {
        var query = new GetMembersQuery { OrderBy = orderBy };
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.OrderBy);
    }

    [Theory]
    [InlineData("oldest")]
    [InlineData("")]
    [InlineData("Name")]
    public void Validate_ShouldHaveError_WhenOrderByIsUnknown(string orderBy)
    {
        var query = new GetMembersQuery { OrderBy = orderBy };
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.OrderBy)
            .WithErrorMessage("OrderBy must be 'newest', 'name' or 'lastActive'.");
    }
}