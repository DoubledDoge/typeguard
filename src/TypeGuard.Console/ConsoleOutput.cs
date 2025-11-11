using ConsolePrism.Core;

namespace TypeGuard.Console;

using Core.Abstractions;
using Console = System.Console;

/// <summary>
/// Provides console-based output functionality for displaying prompts and error messages to the user.
/// </summary>
public class ConsoleOutput : IOutputProvider
{
    /// <summary>
    /// Asynchronously displays a prompt message to the console.
    /// </summary>
    /// <param name="message">The prompt message that gets displayed.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DisplayPromptAsync(
        string message,
        CancellationToken cancellationToken = default
    ) => await Task.Run(() => DisplayPrompt(message), cancellationToken);

    /// <summary>
    /// Asynchronously displays an error message to the console in color and waits for the user to press 'Enter' as confirmation.
    /// </summary>
    /// <param name="message">The error message that gets displayed.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DisplayErrorAsync(
        string message,
        CancellationToken cancellationToken = default
    ) =>
        await Task.Run(
            () =>
            {
                DisplayError(message);
            },
            cancellationToken
        );

    /// <summary>
    /// Synchronously displays a prompt message to the console.
    /// </summary>
    /// <param name="message">The prompt message that gets displayed.</param>
    public void DisplayPrompt(string message) => Console.Write($"{message}: ");

    /// <summary>
    /// Synchronously displays an error message to the console in color and waits for the user to press 'Enter' as confirmation.
    /// </summary>
    /// <param name="message">The error message that gets displayed.</param>
    public void DisplayError(string message)
    {
        ColorWriter.WriteError($"\n{message}");
        Console.Write("Press Enter to try again... ");
        Console.ReadLine();
        Console.WriteLine();
    }
}
