namespace Domain.Common;

public static class DateOnlyExtensions
{
    public static int CalculateAge(this DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}