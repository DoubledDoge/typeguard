using TypeGuard.Core.Handlers;
using TypeGuard.Core.Interfaces;
using TypeGuard.Core.Rules;

namespace TypeGuard.Core.Builders;

/// <summary>
///     A fluent builder for constructing and configuring a DateTimeOffset input handler with validation rules.
///     Each <c>With*</c> method accumulates a rule onto the internal handler while the rules are evaluated
///     in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date/time/offset format string. If null, any valid DateTimeOffset format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
///     An optional factory for creating the internal <see cref="DateTimeOffsetHandler" />.
///     Defaults to constructing a standard <see cref="DateTimeOffsetHandler" /> from the provided providers.
/// </param>
public class DateTimeOffsetInputBuilder(
	string prompt,
	string? format,
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	Func<string, string, IInputProvider, IOutputProvider, DateTimeOffsetHandler>? validatorFactory =
		null
)
	: BuilderBase<DateTimeOffset, DateTimeOffsetInputBuilder>(
		(validatorFactory ?? ((p, f, i, o) => new DateTimeOffsetHandler(i, o, p, f)))(
			prompt ?? throw new ArgumentNullException(nameof(prompt)),
			format ?? throw new ArgumentNullException(nameof(format)),
			inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
			outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
		)
	)
{
	/// <summary>
	///     Adds a rule that ensures the date/time is within the specified range.
	/// </summary>
	/// <remarks>
	///     Comparison uses <see cref="DateTimeOffset" />'s built-in <see cref="IComparable{T}" />
	///     implementation, which compares absolute instants rather than local clock values, so
	///     <paramref name="min" /> and <paramref name="max" /> may carry any offset without affecting
	///     the comparison.
	/// </remarks>
	/// <param name="min">The minimum acceptable date/time (inclusive).</param>
	/// <param name="max">The maximum acceptable date/time (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="min" /> is greater than <paramref name="max" />.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithRange(
		DateTimeOffset min,
		DateTimeOffset max,
		string? customMessage = null
	) =>
		min > max
			? throw new ArgumentException(
				$"min ({min}) must be less than or equal to max ({max}).",
				nameof(min)
			)
			: AddRule(new RangeRule<DateTimeOffset>(min, max, customMessage));

	/// <summary>
	///     Adds a rule that ensures the date is in the future.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithFutureDate(string? customMessage = null) =>
		AddRule(FutureDateRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date is in the past.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithPastDate(string? customMessage = null) =>
		AddRule(PastDateRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date is today.
	/// </summary>
	/// <remarks>
	///     "Today" is resolved against the value's own offset, not the server's local time zone or UTC.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithTodayDate(string? customMessage = null) =>
		AddRule(TodayRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date is not today.
	/// </summary>
	/// <remarks>
	///     "Today" is resolved against the value's own offset, not the server's local time zone or UTC.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithNotTodayDate(string? customMessage = null) =>
		AddRule(NotTodayRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls on a weekday.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithWeekday(string? customMessage = null) =>
		AddRule(WeekdayRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls on a weekend.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithWeekend(string? customMessage = null) =>
		AddRule(WeekendRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls on the specified day of the week.
	/// </summary>
	/// <param name="dayOfWeek">The specific day of the week the date must fall on.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithDayOfWeek(
		DayOfWeek dayOfWeek,
		string? customMessage = null
	) => AddRule(DayOfWeekRule.ForDateTimeOffset(dayOfWeek, customMessage));

	/// <summary>
	///     Adds a rule that ensures the date is within the specified number of calendar days from today.
	/// </summary>
	/// <remarks>
	///     Counts whole calendar days rather than a fractional elapsed-time window, with "today"
	///     resolved against the value's own offset. See <see cref="WithinDaysRule.ForDateTimeOffset" />
	///     for the full rationale.
	/// </remarks>
	/// <param name="days">The number of calendar days from today. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days" /> is less than or equal to zero.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithinDays(int days, string? customMessage = null) =>
		days <= 0
			? throw new ArgumentOutOfRangeException(
				nameof(days),
				days,
				"days must be greater than zero."
			)
			: AddRule(WithinDaysRule.ForDateTimeOffset(days, customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls in the specified year.
	/// </summary>
	/// <param name="year">The specific year the date must fall in.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithYear(int year, string? customMessage = null) =>
		AddRule(YearRule.ForDateTimeOffset(year, customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls in a leap year.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithLeapYear(string? customMessage = null) =>
		AddRule(LeapYearRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the date falls in the specified month.
	/// </summary>
	/// <param name="month">The specific month the date must fall in.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithMonth(int month, string? customMessage = null) =>
		AddRule(MonthRule.ForDateTimeOffset(month, customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component falls within business hours (9 AM to 5 PM).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithBusinessHours(string? customMessage = null) =>
		AddRule(BusinessHoursRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is before the specified time.
	/// </summary>
	/// <param name="maxTime">The upper time boundary (exclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithTimeBefore(
		TimeSpan maxTime,
		string? customMessage = null
	) => AddRule(BeforeTimeRule.ForDateTimeOffset(maxTime, customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is after the specified time.
	/// </summary>
	/// <param name="minTime">The lower time boundary (exclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithTimeAfter(
		TimeSpan minTime,
		string? customMessage = null
	) => AddRule(AfterTimeRule.ForDateTimeOffset(minTime, customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is at the specified hour.
	/// </summary>
	/// <param name="hour">The required hour (0-23).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="hour" /> is not between 0 and 23.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithHour(int hour, string? customMessage = null) =>
		hour is < 0 or > 23
			? throw new ArgumentOutOfRangeException(
				nameof(hour),
				hour,
				"hour must be between 0 and 23."
			)
			: AddRule(HourRule.ForDateTimeOffset(hour, customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component represents a whole hour (zero minutes and seconds).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithWholeHour(string? customMessage = null) =>
		AddRule(WholeHourRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component represents a whole minute (zero seconds).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithWholeMinute(string? customMessage = null) =>
		AddRule(WholeMinuteRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is a multiple of the specified interval.
	/// </summary>
	/// <param name="increment">The required time increment. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="increment" /> is less than or equal to zero.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithTimeIncrement(
		TimeSpan increment,
		string? customMessage = null
	) =>
		increment <= TimeSpan.Zero
			? throw new ArgumentOutOfRangeException(
				nameof(increment),
				increment,
				"increment must be greater than zero."
			)
			: AddRule(TimeIncrementRule.ForDateTimeOffset(increment, customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is in the AM period.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithAm(string? customMessage = null) =>
		AddRule(AmRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is in the PM period.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithPm(string? customMessage = null) =>
		AddRule(PmRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is midnight (00:00:00).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithMidnight(string? customMessage = null) =>
		AddRule(MidnightRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the time component is noon (12:00:00).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithNoon(string? customMessage = null) =>
		AddRule(NoonRule.ForDateTimeOffset(customMessage));

	/// <summary>
	///     Adds a rule that ensures the offset is exactly the specified value.
	/// </summary>
	/// <param name="offset">The required UTC offset.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithOffset(TimeSpan offset, string? customMessage = null) =>
		AddRule(new OffsetRule(offset, customMessage));

	/// <summary>
	///     Adds a rule that ensures the offset falls within the specified minimum and maximum bounds.
	/// </summary>
	/// <param name="minOffset">The minimum acceptable offset (inclusive).</param>
	/// <param name="maxOffset">The maximum acceptable offset (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">
	///     Thrown when <paramref name="minOffset" /> is greater than
	///     <paramref name="maxOffset" />.
	/// </exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithOffsetRange(
		TimeSpan minOffset,
		TimeSpan maxOffset,
		string? customMessage = null
	) => AddRule(new OffsetRangeRule(minOffset, maxOffset, customMessage));

	/// <summary>
	///     Adds a rule that ensures the offset is zero (i.e. the value represents UTC).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithUtcOffset(string? customMessage = null) =>
		AddRule(new UtcOffsetRule(customMessage));

	/// <summary>
	///     Adds a rule that ensures the offset is consistent with the specified time zone at the
	///     value's own date and time, correctly accounting for daylight saving time.
	/// </summary>
	/// <param name="timeZone">The time zone the value's offset is expected to match. Cannot be null.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="timeZone" /> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithTimeZone(
		TimeZoneInfo timeZone,
		string? customMessage = null
	) => AddRule(new MatchesTimeZoneRule(timeZone, customMessage));

	/// <summary>
	///     Adds a custom validation rule to the handler.
	/// </summary>
	/// <param name="predicate">The function that determines whether a DateTimeOffset value is valid. Cannot be null.</param>
	/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">
	///     Thrown when <paramref name="predicate" /> or <paramref name="errorMessage" />
	///     is null.
	/// </exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public DateTimeOffsetInputBuilder WithCustomRule(
		Func<DateTimeOffset, bool> predicate,
		string errorMessage
	)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(errorMessage);

		return AddRule(new CustomRule<DateTimeOffset>(predicate, errorMessage));
	}
}
