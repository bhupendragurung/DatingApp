using Domain.Common;

namespace Api.Tests.Common;

public sealed class DateOnlyExtensionsTests
{
    private static readonly DateOnly Today= new (2026,1,15);

    [Fact]
    public void Birthday_already_passed_this_year_counts_the_full_age()
    {
        var dateOfBirth= new DateOnly(1995,1,10);
        var age= dateOfBirth.CalculateAge(Today);
        Assert.Equal(31,age);
    }

    [Theory]
    [InlineData(1995, 1, 15, 31)]
    [InlineData(1995, 1, 16, 30)]
    [InlineData(1995, 12, 31, 30)]
    [InlineData(2008, 1, 15, 18)]
    [InlineData(2008, 1, 16, 17)]
    public void Age_only_goes_up_on_the_birthday(int year, int month, int day, int expectedAge)
    {
        var dateOfBirth= new DateOnly(year,month,day);
        var age= dateOfBirth.CalculateAge(Today);
        Assert.Equal(expectedAge,age);
    }
   [Fact]
    public void Born_on_leap_day_turns_a_year_older_on_1_March_in_a_normal_year()
    {
        var dateOfBirth = new DateOnly(2000, 2, 29);

        Assert.Equal(25, dateOfBirth.CalculateAge(new DateOnly(2026, 2, 28)));
        Assert.Equal(26, dateOfBirth.CalculateAge(new DateOnly(2026, 3, 1)));
    }
}