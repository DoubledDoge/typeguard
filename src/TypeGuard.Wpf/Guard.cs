using System.Net;
using System.Numerics;
using System.Windows.Controls;

namespace TypeGuard.Wpf;

using Core.Builders;

/// <summary>
/// Provides a concise API for creating configured input builders for WPF applications.
/// </summary>
/// <remarks>
/// <para>
/// Each method returns a fluent builder that can be configured with validation rules via
/// <c>With*</c> methods before calling
/// <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync"/> to prompt the user and retrieve
/// a validated value.
/// </para>
/// <para>
/// Always prefer <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync"/> over
/// <see cref="Core.Builders.BuilderBase{T,TSelf}.Get"/> in WPF. Calling <c>Get()</c> from
/// the UI thread will deadlock because it blocks synchronously over an async operation while
/// holding the synchronization context.
/// </para>
/// <example>
/// <code>
/// private readonly Guard _guard;
///
/// public MyWindow()
/// {
///     InitializeComponent();
///     _guard = new Guard(inputTextBox, promptBlock, errorBlock);
/// }
///
/// private async void SubmitButton_Click(object sender, RoutedEventArgs e)
/// {
///     int age = await _guard.Int("Enter your age")
///         .WithRange(1, 120)
///         .GetAsync();
/// }
/// </code>
/// </example>
/// </remarks>
/// <param name="inputTextBox">The text box to read user input from.</param>
/// <param name="promptBlock">The text block used to display prompt messages.</param>
/// <param name="errorBlock">The text block used to display error messages.</param>
public class Guard(TextBox inputTextBox, TextBlock promptBlock, TextBlock errorBlock)
{
    private readonly WpfInput _input = new(inputTextBox);
    private readonly WpfOutput _output = new(promptBlock, errorBlock);

    /// <summary>
    /// Creates a builder for handling <see cref="int"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<int> Int(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="double"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<double> Double(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="decimal"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<decimal> Decimal(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling numeric input of any type that implements
    /// <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common numeric types such as <see cref="float"/>, <see cref="long"/>,
    /// <see cref="byte"/>, <see cref="Half"/>, and so on.
    /// </summary>
    /// <typeparam name="T">The numeric type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<T> Numeric<T>(string prompt)
        where T : INumber<T>, IMinMaxValue<T> => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling integer input of any type that implements
    /// <see cref="IBinaryInteger{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common integer types such as <see cref="long"/>, <see cref="short"/>,
    /// <see cref="byte"/>, and so on.
    /// </summary>
    /// <typeparam name="T">The integer type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<T> Integer<T>(string prompt)
        where T : IBinaryInteger<T>, IMinMaxValue<T> => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="string"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public StringInputBuilder String(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="char"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public CharInputBuilder Char(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="DateOnly"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
    public DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="DateTime"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date and time format string. If null, any valid DateTime format is accepted.</param>
    public DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="TimeOnly"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
    public TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="TimeSpan"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
    public TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
        new(prompt, format, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="System.Guid"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public GuidInputBuilder Guid(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="IPAddress"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IpAddressInputBuilder IpAddress(string prompt) => new(prompt, _input, _output);

    /// <summary>
    /// Creates a builder for handling <see cref="Uri"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute"/>.</param>
    public UriInputBuilder Uri(string prompt, UriKind uriKind = UriKind.RelativeOrAbsolute) =>
        new(prompt, uriKind, _input, _output);

    /// <summary>
    /// Creates a builder for handling enum input of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
    public EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
        where TEnum : struct, Enum => new(prompt, ignoreCase, _input, _output);
}
