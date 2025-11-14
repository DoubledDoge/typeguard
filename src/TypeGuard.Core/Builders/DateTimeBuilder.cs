namespace TypeGuard.Core.Builders;
using Abstractions;
using Validators;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a DateTime validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class DateTimeBuilder(
	string          prompt,
	string?         format,
	IInputProvider  inputProvider,
	IOutputProvider outputProvider)
{
	private readonly DateTimeValidator _validator = new(inputProvider, outputProvider, prompt, format);

	/// <summary>
	/// Adds a rule that ensures that the date is within the specified range.
	/// </summary>
	/// <param name="min">The minimum acceptable date/time.</param>
	/// <param name="max">The maximum acceptable date/time.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public DateTimeBuilder WithRange(DateTime min, DateTime max, string? customMessage = null)
	{
		_validator.AddRule(new RangeRule<DateTime>(min, max, customMessage));
		return this;
	}

    /// <summary>
    /// Adds a rule that ensures that the date is in the future.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithFutureDate(string? customMessage = null)
    {
        _validator.AddRule(new FutureDateRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is in the past.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithPastDate(string? customMessage = null)
    {
        _validator.AddRule(new PastDateRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithTodayDate(string? customMessage = null)
    {
        _validator.AddRule(new TodayRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is not today.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithNotTodayDate(string? customMessage = null)
    {
        _validator.AddRule(new NotTodayRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on a weekday.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithWeekday(string? customMessage = null)
    {
        _validator.AddRule(new WeekdayRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on a weekend.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithWeekend(string? customMessage = null)
    {
        _validator.AddRule(new WeekendRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls on the specified day of the week.
    /// </summary>
    /// <param name="dayOfWeek">The specific day of the week that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns></returns>
    public DateTimeBuilder WithDayOfWeek(DayOfWeek dayOfWeek, string? customMessage = null)
    {
        _validator.AddRule(new DayOfWeekRule(dayOfWeek, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date is within the specified number of days from today.
    /// </summary>
    /// <param name="days">The number of days from today.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithinDays(int days, string? customMessage = null)
    {
        _validator.AddRule(new WithinDaysRule(days, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls in the specified year.
    /// </summary>
    /// <param name="year">The specific year that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithYear(int year, string? customMessage = null)
    {
        _validator.AddRule(new YearRule(year, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures that the date falls in a leap year.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithLeapYear(string? customMessage = null)
    {
        _validator.AddRule(new LeapYearRule(customMessage));
        return this;
    }

    /// <summary>
    /// Add a rule that ensures that the date falls in the specified month.
    /// </summary>
    /// <param name="month">The specific month that the date must fall under.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public DateTimeBuilder WithMonth(int month, string? customMessage = null)
    {
        _validator.AddRule(new MonthRule(month, customMessage));
        return this;
    }

	/// <summary>
	/// Adds a custom validation rule to the validator.
	/// </summary>
	/// <param name="predicate">The function that determines whether a DateTime value is valid.</param>
	/// <param name="errorMessage">The error message to display when validation fails.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public DateTimeBuilder WithCustomRule(Func<DateTime, bool> predicate, string errorMessage)
	{
		_validator.AddRule(new CustomRule<DateTime>(predicate, errorMessage));
		return this;
	}

	/// <summary>
	/// Asynchronously prompts the user for input and returns the validated DateTime.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the validated DateTime.</returns>
	public async Task<DateTime> GetAsync(CancellationToken cancellationToken = default) => await _validator.GetValidInputAsync(cancellationToken);

	/// <summary>
	/// Synchronously prompts the user for input and returns the validated DateTime.
	/// </summary>
	/// <returns>The validated DateTime.</returns>
	public DateTime Get() => _validator.GetValidInput();
}
