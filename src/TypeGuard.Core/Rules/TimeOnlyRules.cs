namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time falls within standard business hours (9 AM to 5 PM).
/// </summary>
public static class BusinessHoursRule
{
    private static readonly TimeSpan Open = TimeSpan.FromHours(9);
    private static readonly TimeSpan Close = TimeSpan.FromHours(17);

    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is within business hours (9 AM to 5 PM).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new BusinessHoursRuleImpl<DateTime>(v => v.TimeOfDay, customMessage);

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is within business hours (9 AM to 5 PM).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new BusinessHoursRuleImpl<TimeOnly>(v => v.ToTimeSpan(), customMessage);

    private class BusinessHoursRuleImpl<T>(Func<T, TimeSpan> converter, string? customMessage)
        : RulesBase<T>(
            v => converter(v) >= Open && converter(v) <= Close,
            "Time must be within business hours (9 AM to 5 PM)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is before a specified time.
/// </summary>
public static class BeforeTimeRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is before the specified time.
    /// </summary>
    /// <param name="maxTime">The upper time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(
        TimeSpan maxTime,
        string? customMessage = null
    ) => new BeforeTimeRuleImpl<DateTime>(v => v.TimeOfDay, maxTime, customMessage);

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is before the specified time.
    /// </summary>
    /// <param name="maxTime">The upper time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(
        TimeSpan maxTime,
        string? customMessage = null
    ) => new BeforeTimeRuleImpl<TimeOnly>(v => v.ToTimeSpan(), maxTime, customMessage);

    private class BeforeTimeRuleImpl<T>(
        Func<T, TimeSpan> converter,
        TimeSpan maxTime,
        string? customMessage
    )
        : RulesBase<T>(
            v => converter(v) < maxTime,
            $"Time must be before {maxTime:hh\\:mm}",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is after a specified time.
/// </summary>
public static class AfterTimeRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is after the specified time.
    /// </summary>
    /// <param name="minTime">The lower time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(
        TimeSpan minTime,
        string? customMessage = null
    ) => new AfterTimeRuleImpl<DateTime>(v => v.TimeOfDay, minTime, customMessage);

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is after the specified time.
    /// </summary>
    /// <param name="minTime">The lower time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(
        TimeSpan minTime,
        string? customMessage = null
    ) => new AfterTimeRuleImpl<TimeOnly>(v => v.ToTimeSpan(), minTime, customMessage);

    private class AfterTimeRuleImpl<T>(
        Func<T, TimeSpan> converter,
        TimeSpan minTime,
        string? customMessage
    )
        : RulesBase<T>(
            v => converter(v) > minTime,
            $"Time must be after {minTime:hh\\:mm}",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is at a specific hour.
/// </summary>
public static class HourRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is at the specified hour.
    /// </summary>
    /// <param name="hour">The required hour (0-23).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="hour"/> is not between 0 and 23.</exception>
    public static IValidatorRule<DateTime> ForDateTime(int hour, string? customMessage = null)
    {
        ValidateHour(hour);
        return new HourRuleImpl<DateTime>(v => v.Hour, hour, customMessage);
    }

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is at the specified hour.
    /// </summary>
    /// <param name="hour">The required hour (0-23).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="hour"/> is not between 0 and 23.</exception>
    public static IValidatorRule<TimeOnly> ForTimeOnly(int hour, string? customMessage = null)
    {
        ValidateHour(hour);
        return new HourRuleImpl<TimeOnly>(v => v.Hour, hour, customMessage);
    }

    private static void ValidateHour(int hour)
    {
        if (hour is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(
                nameof(hour),
                hour,
                "hour must be between 0 and 23."
            );
        }
    }

    private class HourRuleImpl<T>(Func<T, int> hourGetter, int hour, string? customMessage)
        : RulesBase<T>(v => hourGetter(v) == hour, $"Time must be at hour {hour}", customMessage);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time represents a whole hour (zero minutes and seconds).
/// </summary>
public static class WholeHourRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time represents a whole hour.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v is { Minute: 0, Second: 0, Millisecond: 0 },
            "Time must be a whole hour (no minutes or seconds)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time represents a whole hour.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v is { Minute: 0, Second: 0, Millisecond: 0 },
            "Time must be a whole hour (no minutes or seconds)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time represents a whole minute (zero seconds).
/// </summary>
public static class WholeMinuteRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time represents a whole minute.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v is { Second: 0, Millisecond: 0 },
            "Time must be a whole minute (no seconds)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time represents a whole minute.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v is { Second: 0, Millisecond: 0 },
            "Time must be a whole minute (no seconds)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is a multiple of a specified interval.
/// </summary>
public static class TimeIncrementRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is a multiple of the specified interval.
    /// </summary>
    /// <param name="increment">The required time increment. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="increment"/> is less than or equal to zero.</exception>
    public static IValidatorRule<DateTime> ForDateTime(
        TimeSpan increment,
        string? customMessage = null
    )
    {
        ValidateIncrement(increment);
        return new TimeIncrementRuleImpl<DateTime>(v => v.TimeOfDay, increment, customMessage);
    }

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is a multiple of the specified interval.
    /// </summary>
    /// <param name="increment">The required time increment. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="increment"/> is less than or equal to zero.</exception>
    public static IValidatorRule<TimeOnly> ForTimeOnly(
        TimeSpan increment,
        string? customMessage = null
    )
    {
        ValidateIncrement(increment);
        return new TimeIncrementRuleImpl<TimeOnly>(v => v.ToTimeSpan(), increment, customMessage);
    }

    private static void ValidateIncrement(TimeSpan increment)
    {
        if (increment <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(increment),
                increment,
                "increment must be greater than zero."
            );
        }
    }

    private class TimeIncrementRuleImpl<T>(
        Func<T, TimeSpan> converter,
        TimeSpan increment,
        string? customMessage
    )
        : RulesBase<T>(
            v => converter(v).Ticks % increment.Ticks == 0,
            $"Time must be in increments of {increment}",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is in the AM period (midnight to noon).
/// </summary>
public static class AmRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is in the AM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v.Hour < 12,
            "Time must be in the AM period (midnight to noon)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is in the AM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v.Hour < 12,
            "Time must be in the AM period (midnight to noon)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is in the PM period (noon to midnight).
/// </summary>
public static class PmRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is in the PM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v.Hour >= 12,
            "Time must be in the PM period (noon to midnight)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is in the PM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v.Hour >= 12,
            "Time must be in the PM period (noon to midnight)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is exactly midnight (00:00:00).
/// </summary>
public static class MidnightRule
{
    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is midnight (00:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v.TimeOfDay == TimeSpan.Zero,
            "Time must be midnight (00:00:00)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is midnight (00:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v == TimeOnly.MinValue,
            "Time must be midnight (00:00:00)",
            customMessage
        );
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a time is exactly noon (12:00:00).
/// </summary>
public static class NoonRule
{
    private static readonly TimeSpan _noon = TimeSpan.FromHours(12);
    private static readonly TimeOnly _noonTimeOnly = new(12, 0, 0);

    /// <summary>
    /// Creates a validation rule for DateTime values that ensures the time is noon (12:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for DateTime.</returns>
    public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
        new RulesBase<DateTime>(
            v => v.TimeOfDay == _noon,
            "Time must be noon (12:00:00)",
            customMessage
        );

    /// <summary>
    /// Creates a validation rule for TimeOnly values that ensures the time is noon (12:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>A validation rule for TimeOnly.</returns>
    public static IValidatorRule<TimeOnly> ForTimeOnly(string? customMessage = null) =>
        new RulesBase<TimeOnly>(
            v => v == _noonTimeOnly,
            "Time must be noon (12:00:00)",
            customMessage
        );
}
