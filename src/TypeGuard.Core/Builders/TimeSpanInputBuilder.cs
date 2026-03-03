namespace TypeGuard.Core.Builders;

using Interfaces;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a TimeSpan validator with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">Optional format hint for parsing. If null, any valid TimeSpan format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="TimeSpanValidator"/>.
/// Defaults to constructing a standard <see cref="TimeSpanValidator"/> from the provided providers.
/// </param>
public class TimeSpanInputBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, string, IInputProvider, IOutputProvider, TimeSpanValidator>? validatorFactory =
        null
)
    : BuilderBase<TimeSpan, TimeSpanInputBuilder>(
        (validatorFactory ?? ((p, f, i, o) => new TimeSpanValidator(i, o, p, f)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            format ?? throw new ArgumentNullException(nameof(format)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable TimeSpan (inclusive).</param>
    /// <param name="max">The maximum acceptable TimeSpan (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithRange(TimeSpan min, TimeSpan max, string? customMessage = null)
    {
        return min > max
            ? throw new ArgumentException(
                $"min ({min}) must be less than or equal to max ({max}).",
                nameof(min)
            )
            : this.AddRule(new RangeRule<TimeSpan>(min, max, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is positive.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithPositive(string? customMessage = null) =>
        this.AddRule(new PositiveTimeSpanRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan does not exceed the specified maximum duration.
    /// </summary>
    /// <param name="maximum">The maximum acceptable duration (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithMaxDuration(TimeSpan maximum, string? customMessage = null) =>
        this.AddRule(new MaxDurationRule(maximum, customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan meets the specified minimum duration.
    /// </summary>
    /// <param name="minimum">The minimum acceptable duration (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithMinDuration(TimeSpan minimum, string? customMessage = null) =>
        this.AddRule(new MinDurationRule(minimum, customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan does not exceed the specified number of working hours.
    /// </summary>
    /// <param name="maxHours">The maximum number of hours for a work period. Must be greater than zero. Defaults to 8.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxHours"/> is less than or equal to zero.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithWorkingHours(int maxHours = 8, string? customMessage = null)
    {
        return maxHours <= 0
            ? throw new ArgumentOutOfRangeException(
                nameof(maxHours),
                maxHours,
                "maxHours must be greater than zero."
            )
            : this.AddRule(new WorkingHoursRule(maxHours, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan represents whole hours (no minutes or seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithWholeHours(string? customMessage = null) =>
        this.AddRule(new WholeHoursRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan represents whole minutes (no seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithWholeMinutes(string? customMessage = null) =>
        this.AddRule(new WholeMinutesRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is a multiple of the specified unit.
    /// </summary>
    /// <param name="unit">The unit TimeSpan the value must be a multiple of. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="unit"/> is less than or equal to zero.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithDurationIncrement(TimeSpan unit, string? customMessage = null)
    {
        return unit <= TimeSpan.Zero
            ? throw new ArgumentOutOfRangeException(
                nameof(unit),
                unit,
                "unit must be greater than zero."
            )
            : this.AddRule(new DurationIncrementRule(unit, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the TimeSpan is within a single day (less than 24 hours).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithinDay(string? customMessage = null) =>
        this.AddRule(new WithinDayRule(customMessage));

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a TimeSpan is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public TimeSpanInputBuilder WithCustomRule(Func<TimeSpan, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<TimeSpan>(predicate, errorMessage));
    }
}
