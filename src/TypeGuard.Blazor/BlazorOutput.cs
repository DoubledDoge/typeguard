namespace TypeGuard.Blazor;

using Core.Interfaces;

/// <summary>
/// Provides Blazor-based output by updating bound string properties that a component
/// can reference in its markup, triggering a re-render via a <see cref="OnStateChanged"/>
/// callback.
/// </summary>
/// <remarks>
/// Register a callback pointing at <c>StateHasChanged</c> from your component and bind
/// <see cref="PromptMessage"/> and <see cref="ErrorMessage"/> in your markup:
/// <code>
/// protected override void OnInitialized()
/// {
///     _guard.Output.OnStateChanged = StateHasChanged;
/// }
/// </code>
/// <code>
/// &lt;p&gt;@_guard.Output.PromptMessage&lt;/p&gt;
/// &lt;p style="color: red"&gt;@_guard.Output.ErrorMessage&lt;/p&gt;
/// </code>
/// </remarks>
public class BlazorOutput : IOutputProvider
{
	/// <summary>
	/// Gets the current prompt message. Bind this in your component markup to display the
	/// active prompt to the user.
	/// </summary>
	public string? PromptMessage { get; private set; }

	/// <summary>
	/// Gets the current error message. Bind this in your component markup to display validation
	/// errors to the user. This is null when no error is present.
	/// </summary>
	public string? ErrorMessage { get; private set; }

	/// <summary>
	/// Gets or sets the callback invoked after each state update to notify the component that
	/// it should re-render. Set this to <c>StateHasChanged</c> in your component's
	/// <c>OnInitialized</c> method.
	/// </summary>
	public Action? OnStateChanged { get; set; }

	/// <summary>
	/// Asynchronously displays a prompt message, clearing any existing error message, and
	/// notifies the component to re-render.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task DisplayPromptAsync(string message, CancellationToken cancellationToken = default)
	{
		DisplayPrompt(message);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Asynchronously displays an error message and notifies the component to re-render.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task DisplayErrorAsync(string message, CancellationToken cancellationToken = default)
	{
		DisplayError(message);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Synchronously displays a prompt message, clearing any existing error message, and
	/// notifies the component to re-render.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	public void DisplayPrompt(string message)
	{
		PromptMessage = message;
		ErrorMessage = null;
		OnStateChanged?.Invoke();
	}

	/// <summary>
	/// Synchronously displays an error message and notifies the component to re-render.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	public void DisplayError(string message)
	{
		ErrorMessage = message;
		OnStateChanged?.Invoke();
	}
}
