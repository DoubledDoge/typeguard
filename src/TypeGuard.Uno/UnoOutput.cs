using Microsoft.UI.Dispatching;
using Windows.UI;

namespace TypeGuard.Uno;

using Core.Interfaces;

/// <summary>
/// Provides Uno-based output by displaying prompts and error messages through
/// <see cref="TextBlock"/> controls.
/// </summary>
/// <param name="promptBlock">The text block used to display prompt messages. Cannot be null.</param>
/// <param name="errorBlock">The text block used to display error messages. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="promptBlock"/> or <paramref name="errorBlock"/> is null.</exception>
public class UnoOutput(TextBlock promptBlock, TextBlock errorBlock) : IOutputProvider
{
	private readonly TextBlock _promptBlock =
		promptBlock ?? throw new ArgumentNullException(nameof(promptBlock));
	private readonly TextBlock _errorBlock =
		errorBlock ?? throw new ArgumentNullException(nameof(errorBlock));

	private static readonly SolidColorBrush ErrorBrush = new(Color.FromArgb(255, 255, 0, 0));

	/// <summary>
	/// Asynchronously displays a prompt message on the prompt text block, clearing any existing
	/// error message.
	/// </summary>
	/// <remarks>
	/// All text block updates are marshalled to the UI thread via
	/// <see cref="DispatcherQueue.TryEnqueue(DispatcherQueueHandler)"/> if called from a
	/// background thread.
	/// </remarks>
	/// <param name="message">The prompt message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task DisplayPromptAsync(string message, CancellationToken cancellationToken = default)
	{
		DisplayPrompt(message);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Asynchronously displays an error message on the error text block in red.
	/// </summary>
	/// <remarks>
	/// All text block updates are marshalled to the UI thread via
	/// <see cref="DispatcherQueue.TryEnqueue(DispatcherQueueHandler)"/> if called from a
	/// background thread.
	/// </remarks>
	/// <param name="message">The error message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task DisplayErrorAsync(string message, CancellationToken cancellationToken = default)
	{
		DisplayError(message);
		return Task.CompletedTask;
	}

	/// <summary>
	/// Synchronously displays a prompt message on the prompt text block, clearing any existing
	/// error message.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	public void DisplayPrompt(string message) =>
		RunOnUiThread(() =>
		{
			_promptBlock.Text = message;
			_errorBlock.Text = string.Empty;
		});

	/// <summary>
	/// Synchronously displays an error message on the error text block in red.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	public void DisplayError(string message) =>
		RunOnUiThread(() =>
		{
			_errorBlock.Foreground = ErrorBrush;
			_errorBlock.Text = message;
		});

	private void RunOnUiThread(Action action)
	{
		if (_promptBlock.DispatcherQueue.HasThreadAccess)
		{
			action();
		}
		else
		{
			TaskCompletionSource tcs = new();
			_promptBlock.DispatcherQueue.TryEnqueue(() =>
			{
				action();
				tcs.SetResult();
			});
			tcs.Task.GetAwaiter().GetResult();
		}
	}
}
