namespace TypeGuard.Console;

using Core.Builders;
using Core.Validators;

/// <summary>
/// Provides convenient static methods for validating user input from the console with various different validation rules.
/// </summary>
public static class TypeGuard
{
    private static readonly ConsoleInput DefaultInputProvider = new();
    private static readonly ConsoleOutput DefaultOutputProvider = new();

    /// <summary>
    /// Asynchronously prompts the user for an integer value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated integer.</returns>
    public static async Task<int> GetIntAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        IntValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Synchronously prompts the user for an integer value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated integer.</returns>
    public static int GetInt(string prompt)
    {
        IntValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return validator.GetValidInput();
    }

    /// <summary>
    /// Synchronously prompts the user for a string value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated non-empty string.</returns>
    public static string GetString(string prompt)
    {
        StringValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return validator.GetValidInput();
    }

    /// <summary>
    /// Creates a fluent builder for constructing an integer validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>An <see cref="IntValidatorBuilder"/> instance for configuring validation rules.</returns>
    public static IntValidatorBuilder ForInt(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Creates a fluent builder for constructing a string validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="StringValidatorBuilder"/> instance for configuring validation rules.</returns>
    public static StringValidatorBuilder ForString(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Creates a fluent builder for constructing a DateTime validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date/time format string. If its null, any valid DateTime format is accepted.</param>
    /// <returns>A <see cref="DateTimeValidatorBuilder"/> instance for configuring validation rules.</returns>
    public static DateTimeValidatorBuilder ForDateTime(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);
}
