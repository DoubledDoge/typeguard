namespace TypeGuard.Core.Builders;

using Interfaces;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a DateTime validator with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="DateTimeValidator"/>.
/// Defaults to constructing a standard <see cref="DateTimeValidator"/> from the provided providers.
/// </param>
public class DateTimeInputBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, string, IInputProvider, IOutputProvider, DateTimeValidator>? validatorFactory =
        null
)
    : BuilderBase<DateTime, DateTimeInputBuilder>(
        (validatorFactory ?? ((p, f, i, o) => new DateTimeValidator(i, o, p, f)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            format ?? throw new ArgumentNullException(nameof(format)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a rule that ensures the date/time is within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable date/time (inclusive).</param>
    /// <param name="max">The maximum acceptable date/time (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithRange(DateTime min, DateTime max, string? customMessage = null)
    {
        return min > max
            ? throw new ArgumentException(
                $"min ({min}) must be less than or equal to max ({max}).",
                nameof(min)
            )
            : this.AddRule(new RangeRule<DateTime>(min, max, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithFutureDate(string? customMessage = null) =>
        this.AddRule(FutureDateRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithPastDate(string? customMessage = null) =>
        this.AddRule(PastDateRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithTodayDate(string? customMessage = null) =>
        this.AddRule(TodayRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithNotTodayDate(string? customMessage = null) =>
        this.AddRule(NotTodayRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithWeekday(string? customMessage = null) =>
        this.AddRule(WeekdayRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithWeekend(string? customMessage = null) =>
        this.AddRule(WeekendRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The specific day of the week the date must fall on.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithDayOfWeek(DayOfWeek dayOfWeek, string? customMessage = null) =>
        this.AddRule(DayOfWeekRule.ForDateTime(dayOfWeek, customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is within the specified number of days from today.
    /// </summary>
    /// <param name="days">The number of days from today. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days"/> is less than or equal to zero.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithinDays(int days, string? customMessage = null)
    {
        return days <= 0
            ? throw new ArgumentOutOfRangeException(
                nameof(days),
                days,
                "days must be greater than zero."
            )
            : this.AddRule(WithinDaysRule.ForDateTime(days, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the date falls in the specified year.
    /// </summary>
    /// <param name="year">The specific year the date must fall in.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithYear(int year, string? customMessage = null) =>
        this.AddRule(YearRule.ForDateTime(year, customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls in a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithLeapYear(string? customMessage = null) =>
        this.AddRule(LeapYearRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls in the specified month.
    /// </summary>
    /// <param name="month">The specific month the date must fall in.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithMonth(int month, string? customMessage = null) =>
        this.AddRule(MonthRule.ForDateTime(month, customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component falls within business hours (9 AM to 5 PM).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithBusinessHours(string? customMessage = null) =>
        this.AddRule(BusinessHoursRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is before the specified time.
    /// </summary>
    /// <param name="maxTime">The upper time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithTimeBefore(TimeSpan maxTime, string? customMessage = null) =>
        this.AddRule(BeforeTimeRule.ForDateTime(maxTime, customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is after the specified time.
    /// </summary>
    /// <param name="minTime">The lower time boundary (exclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithTimeAfter(TimeSpan minTime, string? customMessage = null) =>
        this.AddRule(AfterTimeRule.ForDateTime(minTime, customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is at the specified hour.
    /// </summary>
    /// <param name="hour">The required hour (0-23).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="hour"/> is not between 0 and 23.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithHour(int hour, string? customMessage = null)
    {
        return hour is < 0 or > 23
            ? throw new ArgumentOutOfRangeException(
                nameof(hour),
                hour,
                "hour must be between 0 and 23."
            )
            : this.AddRule(HourRule.ForDateTime(hour, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the time component represents a whole hour (zero minutes and seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithWholeHour(string? customMessage = null) =>
        this.AddRule(WholeHourRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component represents a whole minute (zero seconds).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithWholeMinute(string? customMessage = null) =>
        this.AddRule(WholeMinuteRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is a multiple of the specified interval.
    /// </summary>
    /// <param name="increment">The required time increment. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="increment"/> is less than or equal to zero.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithTimeIncrement(TimeSpan increment, string? customMessage = null)
    {
        return increment <= TimeSpan.Zero
            ? throw new ArgumentOutOfRangeException(
                nameof(increment),
                increment,
                "increment must be greater than zero."
            )
            : this.AddRule(TimeIncrementRule.ForDateTime(increment, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the time component is in the AM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithAm(string? customMessage = null) =>
        this.AddRule(AmRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is in the PM period.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithPm(string? customMessage = null) =>
        this.AddRule(PmRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is midnight (00:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithMidnight(string? customMessage = null) =>
        this.AddRule(MidnightRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a rule that ensures the time component is noon (12:00:00).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithNoon(string? customMessage = null) =>
        this.AddRule(NoonRule.ForDateTime(customMessage));

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a DateTime value is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateTimeInputBuilder WithCustomRule(Func<DateTime, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<DateTime>(predicate, errorMessage));
    }
}
