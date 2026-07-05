namespace TypeGuard.WinForms;

using Core.Interfaces;

/// <summary>
/// Provides WinForms-based output by displaying prompts and error messages through
/// <see cref="Label"/> controls.
/// </summary>
/// <param name="promptLabel">The label used to display prompt messages. Cannot be null.</param>
/// <param name="errorLabel">The label used to display error messages. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="promptLabel"/> or <paramref name="errorLabel"/> is null.</exception>
public class WinFormsOutput(Label promptLabel, Label errorLabel) : IOutputProvider
{
	private readonly Label _promptLabel =
		promptLabel ?? throw new ArgumentNullException(nameof(promptLabel));
	private readonly Label _errorLabel =
		errorLabel ?? throw new ArgumentNullException(nameof(errorLabel));

	/// <summary>
	/// Asynchronously displays a prompt message on the prompt label, clearing any existing
	/// error message.
	/// </summary>
	/// <remarks>
	/// All label updates are marshalled to the UI thread via
	/// <see cref="Control.Invoke(Action)"/> if called from a background thread.
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
	/// Asynchronously displays an error message on the error label in red.
	/// </summary>
	/// <remarks>
	/// All label updates are marshalled to the UI thread via
	/// <see cref="Control.Invoke(Action)"/> if called from a background thread.
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
	/// Synchronously displays a prompt message on the prompt label, clearing any existing
	/// error message.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	public void DisplayPrompt(string message)
	{
		if (_promptLabel.InvokeRequired)
		{
			_promptLabel.Invoke(Update);
		}
		else
		{
			Update();
		}

		return;

		void Update()
		{
			_promptLabel.Text = message;
			_errorLabel.Text = string.Empty;
			_errorLabel.ForeColor = SystemColors.ControlText;
		}
	}

	/// <summary>
	/// Synchronously displays an error message on the error label in red.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	public void DisplayError(string message)
	{
		if (_errorLabel.InvokeRequired)
		{
			_errorLabel.Invoke(Update);
		}
		else
		{
			Update();
		}

		return;

		void Update()
		{
			_errorLabel.ForeColor = Color.Red;
			_errorLabel.Text = message;
		}
	}
}
