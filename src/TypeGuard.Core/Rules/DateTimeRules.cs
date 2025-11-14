namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a DateTime is in the future (after the current moment).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class FutureDateRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is in the future.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is after the current moment; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value > DateTime.Now;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be in the future";
}

/// <summary>
/// A validation rule that ensures a DateTime is in the past (before the current moment).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PastDateRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is in the past.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is before the current moment; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value < DateTime.Now;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be in the past";
}

/// <summary>
/// A validation rule that ensures a DateTime is today (same date as the current date, ignoring time).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class TodayRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is today.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime's date matches today's date; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.Date == DateTime.Today;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be today";
}

/// <summary>
/// A validation rule that ensures a DateTime is not today (same date as the current date, ignoring time).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotTodayRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is not today.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime's date does not match today's date; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.Date != DateTime.Today;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must not be today";
}

/// <summary>
/// A validation rule that ensures a DateTime falls on a weekday (Monday through Friday).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WeekdayRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime falls on a weekday.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is a weekday; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.DayOfWeek != DayOfWeek.Saturday && value.DayOfWeek != DayOfWeek.Sunday;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be a weekday";
}

/// <summary>
/// A validation rule that ensures a DateTime falls on a weekend (Saturday or Sunday).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WeekendRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime falls on a weekend.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is a weekend day; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be a weekend day";
}

/// <summary>
/// A validation rule that ensures a DateTime falls on a specific day of the week.
/// </summary>
/// <param name="dayOfWeek">The required day of the week.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DayOfWeekRule(DayOfWeek dayOfWeek, string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime falls on the required day of the week.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is on the specified day of the week; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.DayOfWeek == dayOfWeek;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Date must be a {dayOfWeek}";
}

/// <summary>
/// A validation rule that ensures a DateTime is within a specified number of days from now.
/// </summary>
/// <param name="days">The maximum number of days from the current date.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WithinDaysRule(int days, string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is within the specified number of days from now.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is within the specified days; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value)
    {
        TimeSpan difference = value - DateTime.Now;
        return Math.Abs(difference.TotalDays) <= days;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Date must be within {days} days from now";
}

/// <summary>
/// A validation rule that ensures a DateTime represents a specific year.
/// </summary>
/// <param name="year">The required year.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class YearRule(int year, string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is in the required year.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is in the specified year; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.Year == year;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Date must be in the year {year}";
}

/// <summary>
/// A validation rule that ensures a DateTime falls within a leap year.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LeapYearRule(string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime falls within a leap year.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is in a leap year; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => DateTime.IsLeapYear(value.Year);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Date must be in a valid leap year";
}

/// <summary>
/// A validation rule that ensures a DateTime represents a specific month.
/// </summary>
/// <param name="month">The required month.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MonthRule(int month, string? customMessage = null) : IValidationRule<DateTime>
{
    /// <summary>
    /// Determines whether the specified DateTime is in the required month.
    /// </summary>
    /// <param name="value">The DateTime value to validate.</param>
    /// <returns><c>true</c> if the DateTime is in the specified month; otherwise, <c>false</c>.</returns>
    public bool IsValid(DateTime value) => value.Month == month;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Date must be in month {month}";
}
