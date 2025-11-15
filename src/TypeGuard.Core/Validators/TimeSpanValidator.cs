namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates TimeSpan input.
/// Accepts various formats including "hh:mm:ss", "d.hh:mm:ss", or total units like "5.5".
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">Optional format hint for parsing. If null, accepts any valid TimeSpan format.</param>
public class TimeSpanValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    string? format = null
) : ValidatorBase<TimeSpan>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into a TimeSpan value. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed TimeSpan if parsing succeeded, or <see cref="TimeSpan.Zero"/> if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid TimeSpan; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out TimeSpan value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a valid time span";
            value = TimeSpan.Zero;
            return false;
        }

        bool success =
            format != null
                ? TimeSpan.TryParseExact(input, format, null, out value)
                : TimeSpan.TryParse(input, out value);

        if (success)
        {
            errorMessage = null;
            return true;
        }

        errorMessage =
            format != null
                ? $"Please enter a valid time span in format: {format}"
                : "Please enter a valid time span (e.g., '1:30:00' or '1.12:00:00')";
        value = TimeSpan.Zero;
        return false;
    }
}
