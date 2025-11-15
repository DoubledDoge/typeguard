namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a TimeSpan validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">Optional format hint for parsing. If null, accepts any valid TimeSpan format.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class TimeSpanBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly TimeSpanValidator _validator = new(
        inputProvider,
        outputProvider,
        prompt,
        format
    );

    /// <summary>
    /// Adds a range validation rule to the validator.
    /// </summary>
    /// <param name="min">The minimum acceptable TimeSpan.</param>
    /// <param name="max">The maximum acceptable TimeSpan.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithRange(TimeSpan min, TimeSpan max, string? customMessage = null)
    {
        _validator.AddRule(new RangeRule<TimeSpan>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is positive.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithPositive(string? customMessage = null)
    {
        _validator.AddRule(new PositiveTimeSpanRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan does not exceed a maximum duration.
    /// </summary>
    /// <param name="maximum">The maximum acceptable duration.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithMaxDuration(TimeSpan maximum, string? customMessage = null)
    {
        _validator.AddRule(new MaxDurationRule(maximum, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan meets a minimum duration.
    /// </summary>
    /// <param name="minimum">The minimum acceptable duration.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithMinDuration(TimeSpan minimum, string? customMessage = null)
    {
        _validator.AddRule(new MinDurationRule(minimum, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is within working hours.
    /// </summary>
    /// <param name="maxHours">The maximum number of hours for a work period. Default is 8.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithWorkingHours(int maxHours = 8, string? customMessage = null)
    {
        _validator.AddRule(new WorkingHoursRule(maxHours, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan represents whole hours (no minutes or seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithWholeHours(string? customMessage = null)
    {
        _validator.AddRule(new WholeHoursRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan represents whole minutes (no seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithWholeMinutes(string? customMessage = null)
    {
        _validator.AddRule(new WholeMinutesRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is a multiple of a specified unit.
    /// </summary>
    /// <param name="unit">The unit TimeSpan that the value must be a multiple of.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithDurationIncrement(TimeSpan unit, string? customMessage = null)
    {
        _validator.AddRule(new DurationIncrementRule(unit, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is within a single day.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithinDay(string? customMessage = null)
    {
        _validator.AddRule(new WithinDayRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a TimeSpan is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public TimeSpanBuilder WithCustomRule(Func<TimeSpan, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<TimeSpan>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated TimeSpan.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated TimeSpan.</returns>
    public async Task<TimeSpan> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated TimeSpan.
    /// </summary>
    /// <returns>The validated TimeSpan.</returns>
    public TimeSpan Get() => _validator.GetValidInput();
}
