using System.Net;
using System.Numerics;

// ReSharper disable MemberCanBePrivate.Global

namespace TypeGuard.Blazor;

using Core.Builders;

/// <summary>
/// Provides a concise API for creating configured input builders for Blazor applications.
/// </summary>
/// <remarks>
/// <para>
/// Register this class as a scoped service in your DI container so that each Blazor circuit
/// (Server) or application instance (WebAssembly) gets its own isolated validation state:
/// </para>
/// <code>
/// builder.Services.AddScoped&lt;Guard&gt;();
/// </code>
/// <para>
/// In your component, inject the guard, bind <see cref="Input"/> and <see cref="Output"/>
/// properties to your markup, and register <c>StateHasChanged</c> as the re-render callback:
/// </para>
/// <code>
/// @inject Guard Guard
///
/// &lt;InputText @bind-Value="Guard.Input.Value" /&gt;
/// &lt;p&gt;@Guard.Output.PromptMessage&lt;/p&gt;
/// &lt;p style="color: red"&gt;@Guard.Output.ErrorMessage&lt;/p&gt;
///
/// @code {
///     protected override void OnInitialized()
///     {
///         Guard.Output.OnStateChanged = StateHasChanged;
///     }
///
///     private async Task SubmitAsync()
///     {
///         int age = await Guard.Int("Enter your age")
///             .WithRange(1, 120)
///             .GetAsync();
///     }
/// }
/// </code>
/// <para>
/// Only <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync"/> is supported in Blazor.
/// Calling <see cref="Core.Builders.BuilderBase{T,TSelf}.Get"/> is not exposed as blocking
/// the Blazor rendering thread, particularly on WebAssembly, will freeze the UI entirely.
/// </para>
/// </remarks>
public class Guard
{
    /// <summary>
    /// Gets the input provider that holds the current bound input value.
    /// Bind <see cref="BlazorInput.Value"/> to your input control in markup.
    /// </summary>
    public BlazorInput Input { get; } = new();

    /// <summary>
    /// Gets the output provider that holds the current prompt and error messages.
    /// Bind <see cref="BlazorOutput.PromptMessage"/> and <see cref="BlazorOutput.ErrorMessage"/>
    /// in markup, and set <see cref="BlazorOutput.OnStateChanged"/> to <c>StateHasChanged</c>
    /// in <c>OnInitialized</c>.
    /// </summary>
    public BlazorOutput Output { get; } = new();

    /// <summary>
    /// Creates a builder for handling <see cref="int"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<int> Int(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="double"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<double> Double(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="decimal"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<decimal> Decimal(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling numeric input of any type that implements
    /// <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common numeric types such as <see cref="float"/>, <see cref="long"/>,
    /// <see cref="byte"/>, <see cref="Half"/>, and so on.
    /// </summary>
    /// <typeparam name="T">The numeric type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public NumericInputBuilder<T> Numeric<T>(string prompt)
        where T : INumber<T>, IMinMaxValue<T> => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling integer input of any type that implements
    /// <see cref="IBinaryInteger{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.
    /// Use this for less common integer types such as <see cref="long"/>, <see cref="short"/>,
    /// <see cref="byte"/>, and so on.
    /// </summary>
    /// <typeparam name="T">The integer type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IntegerInputBuilder<T> Integer<T>(string prompt)
        where T : IBinaryInteger<T>, IMinMaxValue<T> => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="string"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public StringInputBuilder String(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="char"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public CharInputBuilder Char(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="DateOnly"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
    public DateOnlyInputBuilder DateOnly(string prompt, string? format = null) =>
        new(prompt, format, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="DateTime"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date and time format string. If null, any valid DateTime format is accepted.</param>
    public DateTimeInputBuilder DateTime(string prompt, string? format = null) =>
        new(prompt, format, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="TimeOnly"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
    public TimeOnlyInputBuilder TimeOnly(string prompt, string? format = null) =>
        new(prompt, format, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="TimeSpan"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
    public TimeSpanInputBuilder TimeSpan(string prompt, string? format = null) =>
        new(prompt, format, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="System.Guid"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public GuidInputBuilder Guid(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="IPAddress"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    public IpAddressInputBuilder IpAddress(string prompt) => new(prompt, Input, Output);

    /// <summary>
    /// Creates a builder for handling <see cref="Uri"/> input.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute"/>.</param>
    public UriInputBuilder Uri(string prompt, UriKind uriKind = UriKind.RelativeOrAbsolute) =>
        new(prompt, uriKind, Input, Output);

    /// <summary>
    /// Creates a builder for handling enum input of type <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to handle.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
    public EnumInputBuilder<TEnum> Enum<TEnum>(string prompt, bool ignoreCase = true)
        where TEnum : struct, Enum => new(prompt, ignoreCase, Input, Output);
}
