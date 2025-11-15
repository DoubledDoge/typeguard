namespace TypeGuard.Core.Rules;

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is in the future.
/// </summary>
public static class FutureDateRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new FutureDateRuleImpl<DateTime>(v => v, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new FutureDateRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), customMessage);

    private class FutureDateRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value) > DateTime.Now;
        public string errorMessage { get; } = customMessage ?? "Date must be in the future";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is in the past.
/// </summary>
public static class PastDateRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new PastDateRuleImpl<DateTime>(v => v, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new PastDateRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), customMessage);

    private class PastDateRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value) < DateTime.Now;
        public string errorMessage { get; } = customMessage ?? "Date must be in the past";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is today.
/// </summary>
public static class TodayRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new TodayRuleImpl<DateTime>(v => v.Date, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new TodayRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue).Date, customMessage);

    private class TodayRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value) == DateTime.Today;
        public string errorMessage { get; } = customMessage ?? "Date must be today";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is not today.
/// </summary>
public static class NotTodayRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new NotTodayRuleImpl<DateTime>(v => v.Date, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new NotTodayRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue).Date, customMessage);

    private class NotTodayRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value) != DateTime.Today;
        public string errorMessage { get; } = customMessage ?? "Date must not be today";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a weekday.
/// </summary>
public static class WeekdayRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new WeekdayRuleImpl<DateTime>(v => v, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new WeekdayRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), customMessage);

    private class WeekdayRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value)
        {
            DateTime dt = converter(value);
            return dt.DayOfWeek is not DayOfWeek.Saturday and DayOfWeek.Sunday;
        }
        public string errorMessage { get; } = customMessage ?? "Date must be a weekday";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a weekend.
/// </summary>
public static class WeekendRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new WeekendRuleImpl<DateTime>(v => v, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new WeekendRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), customMessage);

    private class WeekendRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value)
        {
            DateTime dt = converter(value);
            return dt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }
        public string errorMessage { get; } = customMessage ?? "Date must be a weekend day";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a specific day of the week.
/// </summary>
public static class DayOfWeekRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The required day of the week.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(DayOfWeek dayOfWeek, string? customMessage = null)
        => new DayOfWeekRuleImpl<DateTime>(v => v, dayOfWeek, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The required day of the week.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(DayOfWeek dayOfWeek, string? customMessage = null)
        => new DayOfWeekRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), dayOfWeek, customMessage);

    private class DayOfWeekRuleImpl<T>(Func<T, DateTime> converter, DayOfWeek dayOfWeek, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value).DayOfWeek == dayOfWeek;
        public string errorMessage { get; } = customMessage ?? $"Date must be a {dayOfWeek}";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is within a specified number of days from now.
/// </summary>
public static class WithinDaysRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is within the specified number of days from now.
    /// </summary>
    /// <param name="days">The maximum number of days from the current date.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(int days, string? customMessage = null)
        => new WithinDaysRuleImpl<DateTime>(v => v, days, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is within the specified number of days from now.
    /// </summary>
    /// <param name="days">The maximum number of days from the current date.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(int days, string? customMessage = null)
        => new WithinDaysRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), days, customMessage);

    private class WithinDaysRuleImpl<T>(Func<T, DateTime> converter, int days, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value)
        {
            TimeSpan difference = converter(value) - DateTime.Now;
            return Math.Abs(difference.TotalDays) <= days;
        }
        public string errorMessage { get; } = customMessage ?? $"Date must be within {days} days from now";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date represents a specific year.
/// </summary>
public static class YearRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is in the specified year.
    /// </summary>
    /// <param name="year">The required year.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(int year, string? customMessage = null)
        => new YearRuleImpl<DateTime>(v => v, year, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is in the specified year.
    /// </summary>
    /// <param name="year">The required year.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(int year, string? customMessage = null)
        => new YearRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), year, customMessage);

    private class YearRuleImpl<T>(Func<T, DateTime> converter, int year, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value).Year == year;
        public string errorMessage { get; } = customMessage ?? $"Date must be in the year {year}";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls within a leap year.
/// </summary>
public static class LeapYearRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date falls within a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(string? customMessage = null)
        => new LeapYearRuleImpl<DateTime>(v => v, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date falls within a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(string? customMessage = null)
        => new LeapYearRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), customMessage);

    private class LeapYearRuleImpl<T>(Func<T, DateTime> converter, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => DateTime.IsLeapYear(converter(value).Year);
        public string errorMessage { get; } = customMessage ?? "Date must be in a valid leap year";
    }
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date represents a specific month.
/// </summary>
public static class MonthRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the date is in the specified month.
    /// </summary>
    /// <param name="month">The required month.</param>
    /// <param name="customMessage">An optional custom error message</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidationRule<DateTime> ForDateTime(int month, string? customMessage = null)
        => new MonthRuleImpl<DateTime>(v => v, month, customMessage);

    /// <summary>
    /// Creates a validation rule for DateOnly values that ensures the date is in the specified month.
    /// </summary>
    /// <param name="month">The required month.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateOnly.</returns>
    public static IValidationRule<DateOnly> ForDateOnly(int month, string? customMessage = null)
        => new MonthRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), month, customMessage);

    private class MonthRuleImpl<T>(Func<T, DateTime> converter, int month, string? customMessage) : IValidationRule<T>
    {
        public bool IsValid(T value) => converter(value).Month == month;
        public string errorMessage { get; } = customMessage ?? $"Date must be in month {month}";
    }
}

