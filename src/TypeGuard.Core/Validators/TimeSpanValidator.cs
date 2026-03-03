using TypeGuard.Core.Interfaces;

namespace TypeGuard.Core.Validators;

/// <summary>
/// A validator that prompts for and validates TimeSpan input.
/// Accepts various formats including "hh:mm:ss", "d.hh:mm:ss", or total units like "5.5".
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected time span format string. If null, any valid TimeSpan format is accepted.</param>
public class TimeSpanValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    string? format = null
) : ValidatorBase<TimeSpan>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid TimeSpan matching the expected format; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out TimeSpan value, out string? errorMessage)
    {
        bool success = format is not null
            ? TimeSpan.TryParseExact(input, format, null, out value)
            : TimeSpan.TryParse(input, out value);

        if (success)
        {
            errorMessage = null;
            return true;
        }

        value = default;
        errorMessage = format is not null
            ? $"Please enter a valid time span in the format '{format}'."
            : "Please enter a valid time span (e.g., '1:30:00' or '1.12:00:00').";
        return false;
    }
}
