namespace TypeGuard.Console;

using Core.Interfaces;
using Console = System.Console;

/// <summary>
/// Provides console-based output for displaying prompts and error messages to the user.
/// </summary>
public class ConsoleOutput : IOutputProvider
{
	/// <summary>
	/// Asynchronously displays a prompt message to the console.
	/// </summary>
	/// <remarks>
	/// Because console write operations have no native asynchronous API, this method offloads
	/// the blocking call to a thread pool thread via <see cref="Task.Run(Action, CancellationToken)"/>.
	/// </remarks>
	/// <param name="message">The prompt message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public async Task DisplayPromptAsync(
		string message,
		CancellationToken cancellationToken = default
	) => await Task.Run(() => DisplayPrompt(message), cancellationToken);

	/// <summary>
	/// Asynchronously displays an error message to the console in red and waits for the user to
	/// press Enter before continuing.
	/// </summary>
	/// <remarks>
	/// Because console write operations have no native asynchronous API, this method offloads
	/// the blocking call to a thread pool thread via <see cref="Task.Run(Action, CancellationToken)"/>.
	/// </remarks>
	/// <param name="message">The error message to display.</param>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public async Task DisplayErrorAsync(
		string message,
		CancellationToken cancellationToken = default
	) => await Task.Run(() => DisplayError(message), cancellationToken);

	/// <summary>
	/// Synchronously displays a prompt message to the console, followed by a colon and space.
	/// </summary>
	/// <param name="message">The prompt message to display.</param>
	public void DisplayPrompt(string message) => Console.Write($"{message}: ");

	/// <summary>
	/// Synchronously displays an error message to the console in red and waits for the user to
	/// press Enter before continuing.
	/// </summary>
	/// <param name="message">The error message to display.</param>
	public void DisplayError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"\n{message}");
		Console.ResetColor();

		Console.Write("Press Enter to try again... ");
		Console.ReadLine();
		Console.WriteLine();
	}
}
