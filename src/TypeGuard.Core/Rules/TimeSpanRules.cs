namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a TimeSpan is positive (greater than zero).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PositiveTimeSpanRule(string? customMessage = null)
    : RulesBase<TimeSpan>(v => v > TimeSpan.Zero, "Duration must be positive", customMessage);

/// <summary>
/// A validation rule that ensures a TimeSpan does not exceed the specified maximum duration.
/// </summary>
/// <param name="maximum">The maximum acceptable duration (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MaxDurationRule(TimeSpan maximum, string? customMessage = null)
    : RulesBase<TimeSpan>(v => v <= maximum, $"Duration must not exceed {maximum}", customMessage);

/// <summary>
/// A validation rule that ensures a TimeSpan meets the specified minimum duration.
/// </summary>
/// <param name="minimum">The minimum acceptable duration (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MinDurationRule(TimeSpan minimum, string? customMessage = null)
    : RulesBase<TimeSpan>(v => v >= minimum, $"Duration must be at least {minimum}", customMessage);

/// <summary>
/// A validation rule that ensures a TimeSpan does not exceed the specified number of working hours.
/// </summary>
/// <param name="maxHours">The maximum number of hours for a work period. Must be greater than zero. Defaults to 8.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxHours"/> is less than or equal to zero.</exception>
public class WorkingHoursRule(int maxHours = 8, string? customMessage = null)
    : IValidatorRule<TimeSpan>
{
    private readonly TimeSpan _maxDuration =
        maxHours <= 0
            ? throw new ArgumentOutOfRangeException(
                nameof(maxHours),
                maxHours,
                "maxHours must be greater than zero."
            )
            : TimeSpan.FromHours(maxHours);

    /// <inheritdoc/>
    public bool IsValid(TimeSpan value) => value >= TimeSpan.Zero && value <= _maxDuration;

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Duration must be within {maxHours} working hours";
}

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration in whole hours (no minutes or seconds).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WholeHoursRule(string? customMessage = null)
    : RulesBase<TimeSpan>(
        v => v is { Minutes: 0, Seconds: 0, Milliseconds: 0 },
        "Duration must be in whole hours (no minutes or seconds)",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration in whole minutes (no seconds).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WholeMinutesRule(string? customMessage = null)
    : RulesBase<TimeSpan>(
        v => v is { Seconds: 0, Milliseconds: 0 },
        "Duration must be in whole minutes (no seconds)",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a TimeSpan is a multiple of the specified unit.
/// </summary>
/// <param name="unit">The unit TimeSpan the value must be a multiple of. Must be greater than zero.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="unit"/> is less than or equal to zero.</exception>
public class DurationIncrementRule(TimeSpan unit, string? customMessage = null)
    : IValidatorRule<TimeSpan>
{
    private readonly TimeSpan _unit =
        unit <= TimeSpan.Zero
            ? throw new ArgumentOutOfRangeException(
                nameof(unit),
                unit,
                "unit must be greater than zero."
            )
            : unit;

    /// <inheritdoc/>
    public bool IsValid(TimeSpan value) => value.Ticks % _unit.Ticks == 0;

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Duration must be in increments of {unit}";
}

/// <summary>
/// A validation rule that ensures a TimeSpan represents a duration within a single day (less than 24 hours).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WithinDayRule(string? customMessage = null)
    : RulesBase<TimeSpan>(
        v => v >= TimeSpan.Zero && v < TimeSpan.FromDays(1),
        "Duration must be within a single day (less than 24 hours)",
        customMessage
    );
