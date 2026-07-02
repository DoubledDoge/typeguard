namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is in the future.
/// </summary>
/// <remarks>
/// Validation is performed against <see cref="DateTime.Now"/>, so the rule is sensitive to the
/// current time of day, not just the current date.
/// </remarks>
public static class FutureDateRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is in the future.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(v => v > DateTime.Now, "Date must be in the future", customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is in the future.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.ToDateTime(TimeOnly.MinValue) > DateTime.Now,
			"Date must be in the future",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is in the future.
	/// </summary>
	/// <remarks>
	/// Compares against <see cref="DateTimeOffset.UtcNow"/> rather than <see cref="DateTime.Now"/>,
	/// so the comparison is against the absolute instant and is unaffected by either the
	/// validated value's offset or the local machine's time zone.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v > DateTimeOffset.UtcNow,
			"Date must be in the future",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is in the past.
/// </summary>
/// <remarks>
/// Validation is performed against <see cref="DateTime.Now"/>, so the rule is sensitive to the
/// current time of day, not just the current date.
/// </remarks>
public static class PastDateRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is in the past.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(v => v < DateTime.Now, "Date must be in the past", customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is in the past.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.ToDateTime(TimeOnly.MinValue) < DateTime.Now,
			"Date must be in the past",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is in the past.
	/// </summary>
	/// <remarks>
	/// Compares against <see cref="DateTimeOffset.UtcNow"/> rather than <see cref="DateTime.Now"/>,
	/// so the comparison is against the absolute instant and is unaffected by either the
	/// validated value's offset or the local machine's time zone.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v < DateTimeOffset.UtcNow,
			"Date must be in the past",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is today.
/// </summary>
/// <remarks>
/// Validation is performed against <see cref="DateTime.Today"/>, so the rule is sensitive to the
/// current date boundary only, not the current time of day.
/// </remarks>
public static class TodayRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is today.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(v => v.Date == DateTime.Today, "Date must be today", customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is today.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.ToDateTime(TimeOnly.MinValue).Date == DateTime.Today,
			"Date must be today",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is today.
	/// </summary>
	/// <remarks>
	/// "Today" is resolved against the value's own offset via <see cref="DateTimeOffset.ToOffset"/>,
	/// not the server's local time zone or UTC, so the same absolute instant can be "today" for
	/// one offset and "yesterday" or "tomorrow" for another, matching what a human reading the
	/// value in its own offset would expect.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v.Date == DateTimeOffset.UtcNow.ToOffset(v.Offset).Date,
			"Date must be today",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is not today.
/// </summary>
/// <remarks>
/// Validation is performed against <see cref="DateTime.Today"/>, so the rule is sensitive to the
/// current date boundary only, not the current time of day.
/// </remarks>
public static class NotTodayRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is not today.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(
			v => v.Date != DateTime.Today,
			"Date must not be today",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is not today.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.ToDateTime(TimeOnly.MinValue).Date != DateTime.Today,
			"Date must not be today",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is not today.
	/// </summary>
	/// <remarks>
	/// "Today" is resolved against the value's own offset via <see cref="DateTimeOffset.ToOffset"/>,
	/// not the server's local time zone or UTC. See <see cref="TodayRule.ForDateTimeOffset"/> for
	/// the corresponding rationale.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v.Date != DateTimeOffset.UtcNow.ToOffset(v.Offset).Date,
			"Date must not be today",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a weekday.
/// </summary>
public static class WeekdayRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date falls on a weekday.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(
			v => v.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday),
			"Date must be a weekday",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date falls on a weekday.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday),
			"Date must be a weekday",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date falls on a weekday.
	/// </summary>
	/// <remarks>
	/// Evaluated against <see cref="DateTimeOffset.DayOfWeek"/>, which reflects the date as
	/// represented in the value's own offset (the same "local to this value" semantics
	/// <see cref="DateTime.DayOfWeek"/> uses), not the day of week at UTC.
	/// </remarks>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday),
			"Date must be a weekday",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a weekend.
/// </summary>
public static class WeekendRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date falls on a weekend.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(
			v => v.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
			"Date must be a weekend day",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date falls on a weekend.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => v.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
			"Date must be a weekend day",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date falls on a weekend.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => v.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
			"Date must be a weekend day",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls on a specific day of the week.
/// </summary>
public static class DayOfWeekRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date falls on the specified day of the week.
	/// </summary>
	/// <param name="dayOfWeek">The required day of the week.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(
		DayOfWeek dayOfWeek,
		string? customMessage = null
	) => new DayOfWeekRuleImpl<DateTime>(v => v, dayOfWeek, customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date falls on the specified day of the week.
	/// </summary>
	/// <param name="dayOfWeek">The required day of the week.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(
		DayOfWeek dayOfWeek,
		string? customMessage = null
	) =>
		new DayOfWeekRuleImpl<DateOnly>(
			v => v.ToDateTime(TimeOnly.MinValue),
			dayOfWeek,
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date falls on the specified day of the week.
	/// </summary>
	/// <remarks>
	/// Evaluated against <see cref="DateTimeOffset.DayOfWeek"/> directly, so it does not go
	/// through <see cref="DayOfWeekRuleImpl{T}"/>'s <c>DateTime</c> converter like the other
	/// overloads, since DateTimeOffset already exposes the day of week for its own offset.
	/// </remarks>
	/// <param name="dayOfWeek">The required day of the week.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(
		DayOfWeek dayOfWeek,
		string? customMessage = null
	) =>
		new RulesBase<DateTimeOffset>(
			v => v.DayOfWeek == dayOfWeek,
			$"Date must be a {dayOfWeek}",
			customMessage
		);

	private sealed class DayOfWeekRuleImpl<T>(
		Func<T, DateTime> converter,
		DayOfWeek dayOfWeek,
		string? customMessage
	)
		: RulesBase<T>(
			v => converter(v).DayOfWeek == dayOfWeek,
			$"Date must be a {dayOfWeek}",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date is within a specified number of days from now.
/// </summary>
public static class WithinDaysRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is within the specified number of days from now.
	/// </summary>
	/// <param name="days">The maximum number of days from the current date. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days"/> is less than or equal to zero.</exception>
	public static IValidatorRule<DateTime> ForDateTime(int days, string? customMessage = null) =>
		days <= 0
			? throw new ArgumentOutOfRangeException(
				nameof(days),
				days,
				"days must be greater than zero."
			)
			: new WithinDaysRuleImpl<DateTime>(v => v, days, customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is within the specified number of days from now.
	/// </summary>
	/// <param name="days">The maximum number of days from the current date. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days"/> is less than or equal to zero.</exception>
	public static IValidatorRule<DateOnly> ForDateOnly(int days, string? customMessage = null) =>
		days <= 0
			? throw new ArgumentOutOfRangeException(
				nameof(days),
				days,
				"days must be greater than zero."
			)
			: new WithinDaysRuleImpl<DateOnly>(
				v => v.ToDateTime(TimeOnly.MinValue),
				days,
				customMessage
			);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is within the specified number of calendar days from today.
	/// </summary>
	/// <remarks>
	/// Unlike the <see cref="ForDateTime"/> and <see cref="ForDateOnly"/> overloads, which measure
	/// a fractional elapsed-time window (e.g. exactly 72 hours for <c>days: 3</c>), this overload
	/// counts whole calendar days, with "today" resolved against the value's own offset via
	/// <see cref="DateTimeOffset.ToOffset"/>. This is deliberate: a user picking "within 3 days"
	/// from a date field is almost always thinking in calendar days, not a fractional-hour window,
	/// and the value's own offset determines what day it actually falls on for that value's context.
	/// </remarks>
	/// <param name="days">The maximum number of calendar days from today. Must be greater than zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="days"/> is less than or equal to zero.</exception>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(
		int days,
		string? customMessage = null
	) =>
		days <= 0
			? throw new ArgumentOutOfRangeException(
				nameof(days),
				days,
				"days must be greater than zero."
			)
			: new RulesBase<DateTimeOffset>(
				v =>
					Math.Abs((v.Date - DateTimeOffset.UtcNow.ToOffset(v.Offset).Date).Days) <= days,
				$"Date must be within {days} days from now",
				customMessage
			);

	private sealed class WithinDaysRuleImpl<T>(
		Func<T, DateTime> converter,
		int days,
		string? customMessage
	)
		: RulesBase<T>(
			v => Math.Abs((converter(v) - DateTime.Now).TotalDays) <= days,
			$"Date must be within {days} days from now",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls within a specific year.
/// </summary>
public static class YearRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is in the specified year.
	/// </summary>
	/// <param name="year">The required year. Must be between <see cref="DateTime.MinValue"/>.Year and <see cref="DateTime.MaxValue"/>.Year.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the valid DateTime year range.</exception>
	public static IValidatorRule<DateTime> ForDateTime(int year, string? customMessage = null) =>
		year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year
			? throw new ArgumentOutOfRangeException(
				nameof(year),
				year,
				$"year must be between {DateTime.MinValue.Year} and {DateTime.MaxValue.Year}."
			)
			: new YearRuleImpl<DateTime>(v => v, year, customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is in the specified year.
	/// </summary>
	/// <param name="year">The required year. Must be between <see cref="DateTime.MinValue"/>.Year and <see cref="DateTime.MaxValue"/>.Year.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the valid DateTime year range.</exception>
	public static IValidatorRule<DateOnly> ForDateOnly(int year, string? customMessage = null) =>
		year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year
			? throw new ArgumentOutOfRangeException(
				nameof(year),
				year,
				$"year must be between {DateTime.MinValue.Year} and {DateTime.MaxValue.Year}."
			)
			: new YearRuleImpl<DateOnly>(v => v.ToDateTime(TimeOnly.MinValue), year, customMessage);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is in the specified year.
	/// </summary>
	/// <param name="year">The required year. Must be between <see cref="DateTime.MinValue"/>.Year and <see cref="DateTime.MaxValue"/>.Year.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the valid DateTime year range.</exception>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(
		int year,
		string? customMessage = null
	) =>
		year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year
			? throw new ArgumentOutOfRangeException(
				nameof(year),
				year,
				$"year must be between {DateTime.MinValue.Year} and {DateTime.MaxValue.Year}."
			)
			: new RulesBase<DateTimeOffset>(
				v => v.Year == year,
				$"Date must be in the year {year}",
				customMessage
			);

	private sealed class YearRuleImpl<T>(
		Func<T, DateTime> converter,
		int year,
		string? customMessage
	)
		: RulesBase<T>(
			v => converter(v).Year == year,
			$"Date must be in the year {year}",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls within a leap year.
/// </summary>
public static class LeapYearRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date falls within a leap year.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	public static IValidatorRule<DateTime> ForDateTime(string? customMessage = null) =>
		new RulesBase<DateTime>(
			v => DateTime.IsLeapYear(v.Year),
			"Date must be in a leap year",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date falls within a leap year.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	public static IValidatorRule<DateOnly> ForDateOnly(string? customMessage = null) =>
		new RulesBase<DateOnly>(
			v => DateTime.IsLeapYear(v.Year),
			"Date must be in a leap year",
			customMessage
		);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date falls within a leap year.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(string? customMessage = null) =>
		new RulesBase<DateTimeOffset>(
			v => DateTime.IsLeapYear(v.Year),
			"Date must be in a leap year",
			customMessage
		);
}

/// <summary>
/// Provides factory methods for creating validation rules that ensure a date falls within a specific month.
/// </summary>
public static class MonthRule
{
	/// <summary>
	/// Creates a validation rule for DateTime values that ensures the date is in the specified month.
	/// </summary>
	/// <param name="month">The required month (1-12).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTime.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
	public static IValidatorRule<DateTime> ForDateTime(int month, string? customMessage = null) =>
		month is < 1 or > 12
			? throw new ArgumentOutOfRangeException(
				nameof(month),
				month,
				"month must be between 1 and 12."
			)
			: new MonthRuleImpl<DateTime>(v => v, month, customMessage);

	/// <summary>
	/// Creates a validation rule for DateOnly values that ensures the date is in the specified month.
	/// </summary>
	/// <param name="month">The required month (1-12).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateOnly.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
	public static IValidatorRule<DateOnly> ForDateOnly(int month, string? customMessage = null) =>
		month is < 1 or > 12
			? throw new ArgumentOutOfRangeException(
				nameof(month),
				month,
				"month must be between 1 and 12."
			)
			: new MonthRuleImpl<DateOnly>(
				v => v.ToDateTime(TimeOnly.MinValue),
				month,
				customMessage
			);

	/// <summary>
	/// Creates a validation rule for DateTimeOffset values that ensures the date is in the specified month.
	/// </summary>
	/// <param name="month">The required month (1-12).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>A validation rule for DateTimeOffset.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="month"/> is not between 1 and 12.</exception>
	public static IValidatorRule<DateTimeOffset> ForDateTimeOffset(
		int month,
		string? customMessage = null
	) =>
		month is < 1 or > 12
			? throw new ArgumentOutOfRangeException(
				nameof(month),
				month,
				"month must be between 1 and 12."
			)
			: new RulesBase<DateTimeOffset>(
				v => v.Month == month,
				$"Date must be in month {month}",
				customMessage
			);

	private sealed class MonthRuleImpl<T>(
		Func<T, DateTime> converter,
		int month,
		string? customMessage
	)
		: RulesBase<T>(
			v => converter(v).Month == month,
			$"Date must be in month {month}",
			customMessage
		);
}
