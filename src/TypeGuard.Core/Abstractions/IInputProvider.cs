namespace TypeGuard.Core.Abstractions;

/// <summary>
/// Defines a contract for reading user input from various sources.
/// </summary>
public interface IInputProvider
{
    /// <summary>
    /// Asynchronously retrieves user input as a string.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the input string, or null if no input was provided.</returns>
    Task<string?> GetInputAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously retrieves user input as a string.
    /// </summary>
    /// <returns>The input string, or null if no input was provided.</returns>
    string? GetInput(); // Sync wrapper
}
