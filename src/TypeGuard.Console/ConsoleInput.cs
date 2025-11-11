namespace TypeGuard.Console;

using Console = System.Console;
using Core.Abstractions;

/// <summary>
/// Provides console-based input functionality by reading the user input from the standard input stream.
/// </summary>
public class ConsoleInput : IInputProvider
{
    /// <summary>
    /// Asynchronously reads a line of text from the console and trims any potential whitespace.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the trimmed input string, or null if no input was provided.</returns>
    public async Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
        await Task.Run(GetInput, cancellationToken);

    /// <summary>
    /// Synchronously reads a line of text from the console and trims any potential whitespace.
    /// </summary>
    /// <returns>The trimmed input string, or null if no input was provided.</returns>
    public string? GetInput() => Console.ReadLine()?.Trim();
}
