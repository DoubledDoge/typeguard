using System.Numerics;

namespace TypeGuard.Console;

using Core;
using Core.Builders;

/// <summary>
///     Provides a concise static API for creating configured input builders for console applications.
/// </summary>
/// <remarks>
///     <para>
///         Each method returns a fluent builder that can be configured with validation rules via
///         <c>With*</c> methods before calling <see cref="Core.Builders.BuilderBase{T,TSelf}.Get" />
///         or
///         <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync" /> to prompt the user and retrieve
///         a
///         validated value.
///     </para>
///     <para>
///         If no rules are required, call <c>.Get()</c> directly on the returned builder.
///     </para>
///     <example>
///         <code>
/// int age = Guard.Int("Enter your age")
///     .WithRange(1, 120)
///     .Get();
///
/// string name = Guard.String("Enter your name").Get();
/// </code>
///     </example>
/// </remarks>
public static class Guard
{
	private static readonly ConsoleGuard Instance = new();

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Int" />
	public static IntegerInputBuilder<int> Int(string prompt) => Instance.Int(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Double" />
	public static NumericInputBuilder<double> Double(string prompt) => Instance.Double(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Decimal" />
	public static NumericInputBuilder<decimal> Decimal(string prompt) => Instance.Decimal(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Numeric{T}" />
	public static NumericInputBuilder<T> Numeric<T>(string prompt)
		where T : INumber<T>, IMinMaxValue<T> => Instance.Numeric<T>(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Integer{T}" />
	public static IntegerInputBuilder<T> Integer<T>(string prompt)
		where T : IBinaryInteger<T>, IMinMaxValue<T> => Instance.Integer<T>(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.String" />
	public static StringInputBuilder String(string prompt) => Instance.String(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Char" />
	public static CharInputBuilder Char(string prompt) => Instance.Char(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.DateOnly" />
	public static DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
		Instance.DateOnly(prompt, format);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.DateTime" />
	public static DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
		Instance.DateTime(prompt, format);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.DateTimeOffset" />
	public static DateTimeOffsetInputBuilder DateTimeOffset(string prompt, string? format = null) =>
		Instance.DateTimeOffset(prompt, format);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.TimeOnly" />
	public static TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
		Instance.TimeOnly(prompt, format);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.TimeSpan" />
	public static TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
		Instance.TimeSpan(prompt, format);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Guid" />
	public static GuidInputBuilder Guid(string prompt) => Instance.Guid(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.IpAddress" />
	public static IpAddressInputBuilder IpAddress(string prompt) => Instance.IpAddress(prompt);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Uri" />
	public static UriInputBuilder Uri(
		string prompt,
		UriKind uriKind = UriKind.RelativeOrAbsolute
	) => Instance.Uri(prompt, uriKind);

	/// <inheritdoc cref="GuardBase{TInput,TOutput}.Enum{TEnum}" />
	public static EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
		where TEnum : struct, Enum => Instance.Enum<TEnum>(prompt, ignoreCase);

	private sealed class ConsoleGuard() : GuardBase<ConsoleInput, ConsoleOutput>(new(), new());
}
