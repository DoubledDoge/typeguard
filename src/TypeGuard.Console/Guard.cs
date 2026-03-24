using System.Net;
using System.Numerics;

namespace TypeGuard.Console;

using Core.Builders;

/// <summary>
/// Provides a concise static API for creating configured input builders for console applications.
/// </summary>
/// <remarks>
/// <para>
/// Each method returns a fluent builder that can be configured with validation rules via
/// <c>With*</c> methods before calling <see cref="Core.Builders.BuilderBase{T,TSelf}.Get"/> or
/// <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync"/> to prompt the user and retrieve a
/// validated value.
/// </para>
/// <para>
/// If no rules are required, call <c>.Get()</c> directly on the returned builder.
/// </para>
/// <example>
/// <code>
/// int age = Guard.Int("Enter your age")
///     .WithRange(1, 120)
///     .Get();
///
/// string name = Guard.String("Enter your name").Get();
/// </code>
/// </example>
/// </remarks>
public static class Guard
{
	private static readonly ConsoleInput DefaultInput = new();
	private static readonly ConsoleOutput DefaultOutput = new();

	/// <summary>
	/// Creates a builder for handling <see cref="int"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static IntegerInputBuilder<int> Int(string prompt) =>
		new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="double"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static NumericInputBuilder<double> Double(string prompt) =>
		new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="decimal"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static NumericInputBuilder<decimal> Decimal(string prompt) =>
		new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling numeric input of any type that implements
	/// <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
	/// Use this for less common numeric types such as <see cref="float"/>, <see cref="long"/>,
	/// <see cref="byte"/>, <see cref="Half"/>, and so on.
	/// </summary>
	/// <typeparam name="T">The numeric type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static NumericInputBuilder<T> Numeric<T>(string prompt)
		where T : INumber<T>, IMinMaxValue<T> => new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling integer input of any type that implements
	/// <see cref="IBinaryInteger{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
	/// Use this for less common integer types such as <see cref="long"/>, <see cref="short"/>,
	/// <see cref="byte"/>, and so on.
	/// </summary>
	/// <typeparam name="T">The integer type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static IntegerInputBuilder<T> Integer<T>(string prompt)
		where T : IBinaryInteger<T>, IMinMaxValue<T> => new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="string"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static StringInputBuilder String(string prompt) =>
		new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="char"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static CharInputBuilder Char(string prompt) => new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="DateOnly"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
	public static DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
		new(prompt, format, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="DateTime"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected date and time format string. If null, any valid DateTime format is accepted.</param>
	public static DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
		new(prompt, format, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="TimeOnly"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
	public static TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
		new(prompt, format, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="TimeSpan"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
	public static TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
		new(prompt, format, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="System.Guid"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static GuidInputBuilder Guid(string prompt) => new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="IPAddress"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	public static IpAddressInputBuilder IpAddress(string prompt) =>
		new(prompt, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling <see cref="Uri"/> input.
	/// </summary>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute"/>.</param>
	public static UriInputBuilder Uri(
		string prompt,
		UriKind uriKind = UriKind.RelativeOrAbsolute
	) => new(prompt, uriKind, DefaultInput, DefaultOutput);

	/// <summary>
	/// Creates a builder for handling enum input of type <typeparamref name="TEnum"/>.
	/// </summary>
	/// <typeparam name="TEnum">The enum type to handle.</typeparam>
	/// <param name="prompt">The prompt message to display to the user.</param>
	/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
	public static EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
		where TEnum : struct, Enum => new(prompt, ignoreCase, DefaultInput, DefaultOutput);
}
