using System.Net;
using System.Numerics;
using TypeGuard.Core.Builders;
using TypeGuard.Core.Interfaces;

namespace TypeGuard.Core;

/// <summary>
///     Provides the shared set of input-builder factory methods common to every platform's <c>Guard</c>
///     entry point. Platform packages derive from this to get all builder methods for free, supplying
///     only their own <typeparamref name="TInput" />/<typeparamref name="TOutput" /> providers and any
///     platform-specific constructor requirements (e.g. widget references).
/// </summary>
/// <typeparam name="TInput">The platform's <see cref="IInputProvider" /> implementation.</typeparam>
/// <typeparam name="TOutput">The platform's <see cref="IOutputProvider" /> implementation.</typeparam>
/// <param name="input">The input provider used by every builder this instance creates.</param>
/// <param name="output">The output provider used by every builder this instance creates.</param>
public abstract class GuardBase<TInput, TOutput>(TInput input, TOutput output)
	where TInput : IInputProvider
	where TOutput : IOutputProvider
{
	/// <summary>The input provider shared by every builder this instance creates.</summary>
	protected TInput Input { get; } = input;

	/// <summary>The output provider shared by every builder this instance creates.</summary>
	protected TOutput Output { get; } = output;

	/// <summary>
	///     Creates a builder for handling <see cref="int" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public IntegerInputBuilder<int> Int(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="double" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public NumericInputBuilder<double> Double(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="decimal" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public NumericInputBuilder<decimal> Decimal(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling numeric input of any type that implements
	///     <see cref="INumber{TSelf}" /> and <see cref="IMinMaxValue{TSelf}" />.
	///     Use this for less common numeric types such as <see cref="float" />, <see cref="long" />,
	///     <see cref="byte" />, <see cref="Half" />, and so on.
	/// </summary>
	/// <typeparam name="T">The numeric type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public NumericInputBuilder<T> Numeric<T>(string prompt)
		where T : INumber<T>, IMinMaxValue<T> => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling integer input of any type that implements
	///     <see cref="IBinaryInteger{TSelf}" /> and <see cref="IMinMaxValue{TSelf}" />.
	///     Use this for less common integer types such as <see cref="long" />, <see cref="short" />,
	///     <see cref="byte" />, and so on.
	/// </summary>
	/// <typeparam name="T">The integer type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public IntegerInputBuilder<T> Integer<T>(string prompt)
		where T : IBinaryInteger<T>, IMinMaxValue<T> => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="string" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public StringInputBuilder String(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="char" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public CharInputBuilder Char(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="System.DateOnly" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
	public DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
		new(prompt, format, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="System.DateTime" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected date and time format string. If null, any valid DateTime format is accepted.</param>
	public DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
		new(prompt, format, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="DateTimeOffset" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">
	///     The expected date, time, and offset format string. If null, any valid DateTimeOffset format is
	///     accepted.
	/// </param>
	public DateTimeOffsetInputBuilder DateTimeOffset(string prompt, string? format = null) =>
		new(prompt, format, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="System.TimeOnly" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
	public TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
		new(prompt, format, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="System.TimeSpan" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
	public TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
		new(prompt, format, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="System.Guid" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public GuidInputBuilder Guid(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="IPAddress" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public IpAddressInputBuilder IpAddress(string prompt) => new(prompt, Input, Output);

	/// <summary>
	///     Creates a builder for handling <see cref="Uri" /> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute" />.</param>
	public UriInputBuilder Uri(string prompt, UriKind uriKind = UriKind.RelativeOrAbsolute) =>
		new(prompt, uriKind, Input, Output);

	/// <summary>
	///     Creates a builder for handling enum input of type <typeparamref name="TEnum" />.
	/// </summary>
	/// <typeparam name="TEnum">The enum type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
	public EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
		where TEnum : struct, Enum => new(prompt, ignoreCase, Input, Output);
}
