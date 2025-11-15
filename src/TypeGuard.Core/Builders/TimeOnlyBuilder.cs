namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a TimeOnly validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class TimeOnlyBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly TimeOnlyValidator _validator = new(
        inputProvider,
        outputProvider,
        prompt,
        format
    );

    /// <summary>
    /// Adds a rule that ensures the time is within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable time.</param>
    /// <param name="max">The maximum acceptable time.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithRange(TimeOnly min, TimeOnly max, string? customMessage = null)
    {
        _validator.AddRule(new RangeRule<TimeOnly>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is within business hours (9 AM - 5 PM).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithBusinessHours(string? customMessage = null)
    {
        _validator.AddRule(BusinessHoursRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is before the specified time.
    /// </summary>
    /// <param name="maxTime">The maximum time.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithTimeBefore(TimeSpan maxTime, string? customMessage = null)
    {
        _validator.AddRule(BeforeTimeRule.ForTimeOnly(maxTime, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is after the specified time.
    /// </summary>
    /// <param name="minTime">The minimum time.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithTimeAfter(TimeSpan minTime, string? customMessage = null)
    {
        _validator.AddRule(AfterTimeRule.ForTimeOnly(minTime, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is at the specified hour.
    /// </summary>
    /// <param name="hour">The required hour (0-23).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithHour(int hour, string? customMessage = null)
    {
        _validator.AddRule(HourRule.ForTimeOnly(hour, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time represents whole hours.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithWholeHour(string? customMessage = null)
    {
        _validator.AddRule(WholeHourRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time represents whole minutes.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithWholeMinute(string? customMessage = null)
    {
        _validator.AddRule(WholeMinuteRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is a multiple of the specified interval.
    /// </summary>
    /// <param name="increment">The time increment.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithTimeIncrement(TimeSpan increment, string? customMessage = null)
    {
        _validator.AddRule(TimeIncrementRule.ForTimeOnly(increment, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is in the AM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithAm(string? customMessage = null)
    {
        _validator.AddRule(AmRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is in the PM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithPm(string? customMessage = null)
    {
        _validator.AddRule(PmRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is midnight.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithMidnight(string? customMessage = null)
    {
        _validator.AddRule(MidnightRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the time is noon.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithNoon(string? customMessage = null)
    {
        _validator.AddRule(NoonRule.ForTimeOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a TimeOnly value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeOnlyBuilder WithCustomRule(Func<TimeOnly, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<TimeOnly>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated TimeOnly.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated TimeOnly.</returns>
    public async Task<TimeOnly> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated TimeOnly.
    /// </summary>
    /// <returns>The validated TimeOnly.</returns>
    public TimeOnly Get() => _validator.GetValidInput();
}
