namespace TypeGuard.Blazor;

using Core.Interfaces;

/// <summary>
/// Provides Blazor-based input by reading from a bound string property that a component
/// can bind to via <c>@bind-Value</c>.
/// </summary>
/// <remarks>
/// Bind the <see cref="Value"/> property to an <c>InputText</c> or standard <c>input</c>
/// element in your component markup:
/// <code>
/// &lt;InputText @bind-Value="_guard.Input.Value" /&gt;
/// </code>
/// </remarks>
public class BlazorInput : IInputProvider
{
	/// <summary>
	/// Gets or sets the current input value. Bind this to your input control in the component
	/// markup via <c>@bind-Value</c>.
	/// </summary>
	public string? Value { get; set; }

	/// <summary>
	/// Asynchronously reads the current value of <see cref="Value"/>, trimming any leading or
	/// trailing whitespace.
	/// </summary>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the trimmed value, or null if no value has been set.</returns>
	public Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
		Task.FromResult(GetInput());

	/// <summary>
	/// Synchronously reads the current value of <see cref="Value"/>, trimming any leading or
	/// trailing whitespace.
	/// </summary>
	/// <returns>The trimmed value, or null if no value has been set.</returns>
	public string? GetInput() => string.IsNullOrEmpty(Value) ? null : Value.Trim();
}
