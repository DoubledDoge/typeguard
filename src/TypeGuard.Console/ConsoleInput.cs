namespace TypeGuard.Console;

using Core.Interfaces;
using Console = System.Console;

/// <summary>
/// Provides console-based input by reading from the standard input stream.
/// </summary>
public class ConsoleInput : IInputProvider
{
	/// <summary>
	/// Asynchronously reads a line of text from the console, trimming any leading or trailing whitespace.
	/// </summary>
	/// <remarks>
	/// Because <see cref="Console.ReadLine"/> has no native asynchronous API, this method offloads
	/// the blocking call to a thread pool thread via <see cref="Task.Run(Func{Task?}, CancellationToken)"/>.
	/// </remarks>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the trimmed input string, or null if no input was provided.</returns>
	public async Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
		await Task.Run(GetInput, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Synchronously reads a line of text from the console, trimming any leading or trailing whitespace.
	/// </summary>
	/// <returns>The trimmed input string, or null if no input was provided.</returns>
	public string? GetInput() => Console.ReadLine()?.Trim();
}
