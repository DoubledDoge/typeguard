namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a TimeSpan is positive.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PositiveTimeSpanRule(string? customMessage = null) : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan is positive.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is greater than zero; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value > TimeSpan.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Duration must be positive";
}

/// <summary>
/// A validation rule that ensures a TimeSpan does not exceed a maximum duration.
/// </summary>
/// <param name="maximum">The maximum acceptable duration.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MaxDurationRule(TimeSpan maximum, string? customMessage = null)
    : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan does not exceed the maximum duration.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is less than or equal to the maximum; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value <= maximum;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Duration must not exceed {maximum}";
}

/// <summary>
/// A validation rule that ensures a TimeSpan meets a minimum duration.
/// </summary>
/// <param name="minimum">The minimum acceptable duration.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MinDurationRule(TimeSpan minimum, string? customMessage = null)
    : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan meets the minimum duration.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is greater than or equal to the minimum; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value >= minimum;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Duration must be at least {minimum}";
}

/// <summary>
/// A validation rule that ensures a TimeSpan is within the standard working hours.
/// </summary>
/// <param name="maxHours">The maximum number of hours for a work period. Default is 8.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WorkingHoursRule(int maxHours = 8, string? customMessage = null)
    : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan is within working hours.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is within the working hours limit; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) =>
        value >= TimeSpan.Zero && value <= TimeSpan.FromHours(maxHours);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? $"Duration must be within {maxHours} hours";
}

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration in whole hours (no minutes or seconds).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WholeHoursRule(string? customMessage = null) : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan represents whole hours.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan has no minutes, seconds, or milliseconds; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value is { Minutes: 0, Seconds: 0, Milliseconds: 0 };

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Duration must be in whole hours";
}

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration in whole minutes (no seconds).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WholeMinutesRule(string? customMessage = null) : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan represents whole minutes.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan has no seconds or milliseconds; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value is { Seconds: 0, Milliseconds: 0 };

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Duration must be in whole minutes";
}

/// <summary>
/// A validation rule that ensures a TimeSpan is a multiple of a specified unit.
/// Useful for enforcing time increments.
/// </summary>
/// <param name="unit">The unit TimeSpan that the value must be a multiple of.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DurationIncrementRule(TimeSpan unit, string? customMessage = null)
    : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan is a multiple of the unit.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is evenly divisible by the unit; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value.Ticks % unit.Ticks == 0;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? $"Duration must be in increments of {unit}";
}

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration within a single day.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WithinDayRule(string? customMessage = null) : IValidationRule<TimeSpan>
{
    /// <summary>
    /// Determines whether the specified TimeSpan is less than 24 hours.
    /// </summary>
    /// <param name="value">The TimeSpan value to validate.</param>
    /// <returns><c>true</c> if the TimeSpan is less than 24 hours; otherwise, <c>false</c>.</returns>
    public bool IsValid(TimeSpan value) => value >= TimeSpan.Zero && value < TimeSpan.FromDays(1);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? "Duration must be within a single day (less than 24 hours)";
}
