using System.Globalization;

namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates DateOnly input according to a specified format.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
/// <param name="formatProvider">The format provider to use for parsing. If null, the current culture is used.</param>
/// <param name="dateTimeStyles">The styles to use for parsing. Default is <see cref="DateTimeStyles.None"/>.</param>
public class DateOnlyValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    string? format = null,
    IFormatProvider? formatProvider = null,
    DateTimeStyles dateTimeStyles = DateTimeStyles.None
) : ValidatorBase<DateOnly>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into a DateOnly value using the configured format. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed DateOnly if parsing succeeded, or the default DateOnly value if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input matches the expected format and is a valid DateOnly; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out DateOnly value, out string? errorMessage)
    {
        if (DateOnly.TryParseExact(input, format, formatProvider, dateTimeStyles, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = $"Please enter a valid date in the format {format}";
        value = default(DateOnly);
        return false;
    }
}
