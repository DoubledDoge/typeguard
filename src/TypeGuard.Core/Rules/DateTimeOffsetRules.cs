namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
///     A validation rule that ensures a DateTimeOffset's offset is exactly the specified value.
/// </summary>
/// <param name="offset">The required UTC offset.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class OffsetRule(TimeSpan offset, string? customMessage = null)
	: RulesBase<DateTimeOffset>(
		v => v.Offset == offset,
		$@"Offset must be {offset:hh\:mm}",
		customMessage
	);

/// <summary>
///     A validation rule that ensures a DateTimeOffset's offset falls within the specified minimum
///     and maximum bounds.
/// </summary>
/// <param name="minOffset">The minimum acceptable offset (inclusive).</param>
/// <param name="maxOffset">The maximum acceptable offset (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">
///     Thrown when <paramref name="minOffset" /> is greater than
///     <paramref name="maxOffset" />.
/// </exception>
public class OffsetRangeRule(TimeSpan minOffset, TimeSpan maxOffset, string? customMessage = null)
	: IValidatorRule<DateTimeOffset>
{
	private readonly TimeSpan _minOffset =
		minOffset > maxOffset
			? throw new ArgumentException(
				$"minOffset ({minOffset}) must be less than or equal to maxOffset ({maxOffset}).",
				nameof(minOffset)
			)
			: minOffset;

	/// <inheritdoc />
	public bool IsValid(DateTimeOffset value) =>
		value.Offset >= _minOffset && value.Offset <= maxOffset;

	/// <inheritdoc />
	public string ErrorMessage { get; } =
		customMessage ?? $@"Offset must be between {minOffset:hh\:mm} and {maxOffset:hh\:mm}";
}

/// <summary>
///     A validation rule that ensures a DateTimeOffset's offset is zero (i.e. it represents UTC).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class UtcOffsetRule(string? customMessage = null)
	: RulesBase<DateTimeOffset>(
		v => v.Offset == TimeSpan.Zero,
		"Offset must be UTC (+00:00)",
		customMessage
	);

/// <summary>
///     A validation rule that ensures a DateTimeOffset's offset is consistent with what the specified
///     time zone would produce at that value's own date and time, correctly accounting for daylight
///     saving time.
/// </summary>
/// <remarks>
///     A DateTimeOffset carries only a raw UTC offset, not a time zone identifier: two values with the
///     same offset may represent entirely different zones, and the same zone can legitimately produce
///     different offsets across a DST transition. This rule closes that gap by resolving the expected
///     offset from <paramref name="timeZone" /> at the value's own date and time (via
///     <see cref="TimeZoneInfo.GetUtcOffset(DateTime)" />, which treats the unspecified-kind
///     <see cref="DateTimeOffset.DateTime" /> component as wall-clock time in that zone), rather than
///     comparing against a single fixed offset. A January submission and a July submission from the
///     same zone are validated against different expected offsets when the zone observes DST.
/// </remarks>
/// <param name="timeZone">The time zone the value's offset is expected to match. Cannot be null.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="timeZone" /> is null.</exception>
public class MatchesTimeZoneRule(TimeZoneInfo timeZone, string? customMessage = null)
	: IValidatorRule<DateTimeOffset>
{
	private readonly TimeZoneInfo _timeZone =
		timeZone ?? throw new ArgumentNullException(nameof(timeZone));

	/// <inheritdoc />
	public bool IsValid(DateTimeOffset value) =>
		value.Offset == _timeZone.GetUtcOffset(value.DateTime);

	/// <inheritdoc />
	public string ErrorMessage { get; } =
		customMessage ?? $"Offset must match {timeZone.Id} at the given date and time";
}
