namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a TimeOnly input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="TimeOnlyHandler"/>.
/// Defaults to constructing a standard <see cref="TimeOnlyHandler"/> from the provided providers.
/// </param>
public class TimeOnlyInputBuilder(
	string prompt,
	string? format,
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	Func<string, string, IInputProvider, IOutputProvider, TimeOnlyHandler>? validatorFactory = null
)
	: BuilderBase<TimeOnly, TimeOnlyInputBuilder>(
		(validatorFactory ?? ((p, f, i, o) => new TimeOnlyHandler(i, o, p, f)))(
			prompt ?? throw new ArgumentNullException(nameof(prompt)),
			format ?? throw new ArgumentNullException(nameof(format)),
			inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
			outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
		)
	)
{
	/// <summary>
	/// Adds a rule that ensures the time is within the specified range.
	/// </summary>
	/// <param name="min">The minimum acceptable time (inclusive).</param>
	/// <param name="max">The maximum acceptable time (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithRange(
		TimeOnly min,
		TimeOnly max,
		string? customMessage = null
	) =>
		min > max
			? throw new ArgumentException(
				$"min ({min}) must be less than or equal to max ({max}).",
				nameof(min)
			)
			: AddRule(new RangeRule<TimeOnly>(min, max, customMessage));

	/// <summary>
	/// Adds a rule that ensures the time falls within business hours (9 AM to 5 PM).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithBusinessHours(string? customMessage = null) =>
		AddRule(BusinessHoursRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is before the specified time.
	/// </summary>
	/// <param name="maxTime">The upper time boundary (exclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithTimeBefore(TimeSpan maxTime, string? customMessage = null) =>
		AddRule(BeforeTimeRule.ForTimeOnly(maxTime, customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is after the specified time.
	/// </summary>
	/// <param name="minTime">The lower time boundary (exclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithTimeAfter(TimeSpan minTime, string? customMessage = null) =>
		AddRule(AfterTimeRule.ForTimeOnly(minTime, customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is at the specified hour.
	/// </summary>
	/// <param name="hour">The required hour (0-23).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="hour"/> is not between 0 and 23.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithHour(int hour, string? customMessage = null) =>
		hour is < 0 or > 23
			? throw new ArgumentOutOfRangeException(
				nameof(hour),
				hour,
				"hour must be between 0 and 23."
			)
			: AddRule(HourRule.ForTimeOnly(hour, customMessage));

	/// <summary>
	/// Adds a rule that ensures the time represents a whole hour (zero minutes and seconds).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithWholeHour(string? customMessage = null) =>
		AddRule(WholeHourRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time represents a whole minute (zero seconds).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithWholeMinute(string? customMessage = null) =>
		AddRule(WholeMinuteRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is a multiple of the specified interval.
	/// </summary>
	/// <param name="increment">The required time increment. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="increment"/> is less than or equal to zero.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithTimeIncrement(
		TimeSpan increment,
		string? customMessage = null
	) =>
		increment <= TimeSpan.Zero
			? throw new ArgumentOutOfRangeException(
				nameof(increment),
				increment,
				"increment must be greater than zero."
			)
			: AddRule(TimeIncrementRule.ForTimeOnly(increment, customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is in the AM period.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithAm(string? customMessage = null) =>
		AddRule(AmRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is in the PM period.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithPm(string? customMessage = null) =>
		AddRule(PmRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is midnight (00:00:00).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithMidnight(string? customMessage = null) =>
		AddRule(MidnightRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a rule that ensures the time is noon (12:00:00).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithNoon(string? customMessage = null) =>
		AddRule(NoonRule.ForTimeOnly(customMessage));

	/// <summary>
	/// Adds a custom validation rule to the input handler.
	/// </summary>
	/// <param name="predicate">The function that determines whether a TimeOnly value is valid. Cannot be null.</param>
	/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public TimeOnlyInputBuilder WithCustomRule(Func<TimeOnly, bool> predicate, string errorMessage)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(errorMessage);

		return AddRule(new CustomRule<TimeOnly>(predicate, errorMessage));
	}
}
