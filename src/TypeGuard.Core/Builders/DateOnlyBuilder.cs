namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a DateTime validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class DateOnlyBuilder(
    string prompt,
    string? format,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly DateOnlyValidator _validator = new(
        inputProvider,
        outputProvider,
        prompt,
        format
    );

    /// <summary>
    /// Adds a rule that ensures that the date is within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable date.</param>
    /// <param name="max">The maximum acceptable date.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithRange(DateOnly min, DateOnly max, string? customMessage = null)
    {
        _validator.AddRule(new RangeRule<DateOnly>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithFutureDate(string? customMessage = null)
    {
        _validator.AddRule(FutureDateRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithPastDate(string? customMessage = null)
    {
        _validator.AddRule(PastDateRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithTodayDate(string? customMessage = null)
    {
        _validator.AddRule(TodayRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithNotTodayDate(string? customMessage = null)
    {
        _validator.AddRule(NotTodayRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithWeekday(string? customMessage = null)
    {
        _validator.AddRule(WeekdayRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithWeekend(string? customMessage = null)
    {
        _validator.AddRule(WeekendRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The specific day of the week that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithDayOfWeek(DayOfWeek dayOfWeek, string? customMessage = null)
    {
        _validator.AddRule(DayOfWeekRule.ForDateOnly(dayOfWeek, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is within the specified number of days from today.
    /// </summary>
    /// <param name="days">The number of days from today.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithinDays(int days, string? customMessage = null)
    {
        _validator.AddRule(WithinDaysRule.ForDateOnly(days, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls in the specified year.
    /// </summary>
    /// <param name="year">The specific year that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithYear(int year, string? customMessage = null)
    {
        _validator.AddRule(YearRule.ForDateOnly(year, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls in a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithLeapYear(string? customMessage = null)
    {
        _validator.AddRule(LeapYearRule.ForDateOnly(customMessage));
        return this;
    }

    /// <summary>
    /// Add a rule that ensures that the date falls in the specified month.
    /// </summary>
    /// <param name="month">The specific month that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithMonth(int month, string? customMessage = null)
    {
        _validator.AddRule(MonthRule.ForDateOnly(month, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a DateOnly value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateOnlyBuilder WithCustomRule(Func<DateOnly, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<DateOnly>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated DateOnly.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated DateOnly.</returns>
    public async Task<DateOnly> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated DateOnly.
    /// </summary>
    /// <returns>The validated DateOnly.</returns>
    public DateOnly Get() => _validator.GetValidInput();
}
