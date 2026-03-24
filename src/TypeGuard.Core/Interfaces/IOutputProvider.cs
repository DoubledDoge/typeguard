namespace TypeGuard.Core.Interfaces;

/// <summary>
/// Defines a contract for displaying messages and prompts to the user.
/// </summary>
public interface IOutputProvider
{
	/// <summary>
	/// Asynchronously displays a prompt message to the user.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.
	/// When cancelled, the display operation should terminate gracefully without completing the message output.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DisplayPromptAsync(string message, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously displays an error message to the user.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.
	/// When cancelled, the display operation should terminate gracefully without completing the message output.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task DisplayErrorAsync(string message, CancellationToken cancellationToken = default);

	/// <summary>
	/// Synchronously displays a prompt message to the user.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	void DisplayPrompt(string message);

	/// <summary>
	/// Synchronously displays an error message to the user.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	void DisplayError(string message);
}
