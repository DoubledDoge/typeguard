using System.Net;
using System.Numerics;

namespace TypeGuard.Maui;

using Core.Builders;

/// <summary>
/// Provides a concise API for creating configured input builders for MAUI applications.
/// Supports Windows, macOS, iOS, and Android through MAUI's unified control model.
/// </summary>
/// <remarks>
/// Register this class in your MAUI DI container with the appropriate lifetime.
/// Use AddTransient for per-page validation or AddScoped for shared validation state
/// within a navigation scope:
/// builder.Services.AddTransient&lt;Guard&gt;()
/// <code>
/// _guard = new Guard(InputEntry, PromptLabel, ErrorLabel, SubmitButton);
/// int age = await _guard.Int("Enter your age")
///     .WithRange(1, 120)
///     .GetAsync();
/// </code>
/// </remarks>
/// <param name="entry">The entry control to read user input from.</param>
/// <param name="promptLabel">The label used to display prompt messages.</param>
/// <param name="errorLabel">The label used to display error messages.</param>
/// <param name="submitButton">
/// An optional button that the user taps to submit input. If null, input advances on the
/// keyboard return key instead. Recommended on mobile platforms.
/// </param>
public class Guard(Entry entry, Label promptLabel, Label errorLabel, Button? submitButton = null)
{
    private readonly MauiInput _input = new(entry, submitButton);
    private readonly MauiOutput _output = new(promptLabel, errorLabel);

    /// <summary>Creates a builder for validated <see cref="int"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<int> Int(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="double"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<double> Double(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="decimal"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<decimal> Decimal(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for validated numeric input of any type that implements
    /// <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common numeric types such as float, long, byte, Half, and so on.
    /// </summary>
    /// <typeparam name="T">The numeric type to validate.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<T> Numeric<T>(string prompt)
        where T : INumber<T>, IMinMaxValue<T> => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for validated integer input of any type that implements
    /// <see cref="IBinaryInteger{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common integer types such as long, short, byte, and so on.
    /// </summary>
    /// <typeparam name="T">The integer type to validate.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<T> Integer<T>(string prompt)
        where T : IBinaryInteger<T>, IMinMaxValue<T> => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="string"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public StringInputBuilder String(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="char"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public CharInputBuilder Char(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="DateOnly"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
    public DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>Creates a builder for validated <see cref="DateTime"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date and time format string. If null, any valid DateTime format is accepted.</param>
    public DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>Creates a builder for validated <see cref="TimeOnly"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
    public TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>Creates a builder for validated <see cref="TimeSpan"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
    public TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>Creates a builder for validated <see cref="System.Guid"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public GuidInputBuilder Guid(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="IPAddress"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IpAddressInputBuilder IpAddress(string prompt) => new(prompt, _input, _output);

    /// <summary>Creates a builder for validated <see cref="Uri"/> input.</summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute"/>.</param>
    public UriInputBuilder Uri(string prompt, UriKind uriKind = UriKind.RelativeOrAbsolute) =>
        new(prompt, uriKind, _input, _output);

    /// <summary>Creates a builder for validated enum input of type <typeparamref name="TEnum"/>.</summary>
    /// <typeparam name="TEnum">The enum type to validate.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
    public EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
        where TEnum : struct, Enum => new(prompt, ignoreCase, _input, _output);
}
