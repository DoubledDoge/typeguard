using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TypeGuard.Wpf;

using Core.Interfaces;

/// <summary>
/// Provides WPF-based output by displaying prompts and error messages through
/// <see cref="TextBlock"/> controls.
/// </summary>
/// <param name="promptBlock">The text block used to display prompt messages. Cannot be null.</param>
/// <param name="errorBlock">The text block used to display error messages. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="promptBlock"/> or <paramref name="errorBlock"/> is null.</exception>
public class WpfOutput(TextBlock promptBlock, TextBlock errorBlock) : IOutputProvider
{
	private readonly TextBlock _promptBlock =
		promptBlock ?? throw new ArgumentNullException(nameof(promptBlock));
	private readonly TextBlock _errorBlock =
		errorBlock ?? throw new ArgumentNullException(nameof(errorBlock));

	/// <summary>
	/// Asynchronously displays a prompt message on the prompt text block, clearing any existing
	/// error message.
	/// </summary>
	/// <remarks>
	/// All text block updates are marshalled to the UI thread via
	/// <see cref="System.Windows.Threading.Dispatcher.Invoke(Action)"/> if called from a
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
	/// <see cref="System.Windows.Threading.Dispatcher.Invoke(Action)"/> if called from a
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
	public void DisplayPrompt(string message)
	{
		if (_promptBlock.Dispatcher.CheckAccess())
		{
			Update();
		}
		else
		{
			_promptBlock.Dispatcher.Invoke(Update);
		}

		return;

		void Update()
		{
			_promptBlock.Text = message;
			_errorBlock.Text = string.Empty;
			_errorBlock.Foreground = SystemColors.ControlTextBrush;
		}
	}

	/// <summary>
	/// Synchronously displays an error message on the error text block in red.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	public void DisplayError(string message)
	{
		if (_errorBlock.Dispatcher.CheckAccess())
		{
			Update();
		}
		else
		{
			_errorBlock.Dispatcher.Invoke(Update);
		}

		return;

		void Update()
		{
			_errorBlock.Foreground = Brushes.Red;
			_errorBlock.Text = message;
		}
	}
}
