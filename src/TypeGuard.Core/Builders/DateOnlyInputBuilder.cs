namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a DateOnly input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="DateOnlyHandler"/>.
/// Defaults to constructing a standard <see cref="DateOnlyHandler"/> from the provided providers.
/// </param>
public class DateOnlyInputBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, string, IInputProvider, IOutputProvider, DateOnlyHandler>? validatorFactory = null
)
    : BuilderBase<DateOnly, DateOnlyInputBuilder>(
        (validatorFactory ?? ((p, f, i, o) => new DateOnlyHandler(i, o, p, f)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            format ?? throw new ArgumentNullException(nameof(format)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a rule that ensures the date is within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable date (inclusive).</param>
    /// <param name="max">The maximum acceptable date (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithRange(DateOnly min, DateOnly max, string? customMessage = null)
    {
        return min > max
            ? throw new ArgumentException(
                $"min ({min}) must be less than or equal to max ({max}).",
                nameof(min)
            )
            : this.AddRule(new RangeRule<DateOnly>(min, max, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithFutureDate(string? customMessage = null) =>
        this.AddRule(FutureDateRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithPastDate(string? customMessage = null) =>
        this.AddRule(PastDateRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithTodayDate(string? customMessage = null) =>
        this.AddRule(TodayRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithNotTodayDate(string? customMessage = null) =>
        this.AddRule(NotTodayRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithWeekday(string? customMessage = null) =>
        this.AddRule(WeekdayRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithWeekend(string? customMessage = null) =>
        this.AddRule(WeekendRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The specific day of the week the date must fall on.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithDayOfWeek(DayOfWeek dayOfWeek, string? customMessage = null) =>
        this.AddRule(DayOfWeekRule.ForDateOnly(dayOfWeek, customMessage));

    /// <summary>
    /// Adds a rule that ensures the date is within the specified number of days from today.
    /// </summary>
    /// <param name="days">The number of days from today. Must be greater than zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days"/> is less than or equal to zero.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithinDays(int days, string? customMessage = null)
    {
        return days <= 0
            ? throw new ArgumentOutOfRangeException(
                nameof(days),
                days,
                "days must be greater than zero."
            )
            : this.AddRule(WithinDaysRule.ForDateOnly(days, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the date falls in the specified year.
    /// </summary>
    /// <param name="year">The specific year the date must fall in.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithYear(int year, string? customMessage = null) =>
        this.AddRule(YearRule.ForDateOnly(year, customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls in a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithLeapYear(string? customMessage = null) =>
        this.AddRule(LeapYearRule.ForDateOnly(customMessage));

    /// <summary>
    /// Adds a rule that ensures the date falls in the specified month.
    /// </summary>
    /// <param name="month">The specific month the date must fall in.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithMonth(int month, string? customMessage = null) =>
        this.AddRule(MonthRule.ForDateOnly(month, customMessage));

    /// <summary>
    /// Adds a custom validation rule to the handler.
    /// </summary>
    /// <param name="predicate">The function that determines whether a DateOnly value is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public DateOnlyInputBuilder WithCustomRule(Func<DateOnly, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<DateOnly>(predicate, errorMessage));
    }
}
