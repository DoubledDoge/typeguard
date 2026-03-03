using System.Globalization;

namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// A validator that prompts for and validates DateTime input according to a specified format.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
/// <param name="formatProvider">The format provider to use for parsing. Defaults to the current culture.</param>
/// <param name="dateTimeStyles">The styles to use for parsing. Defaults to <see cref="DateTimeStyles.None"/>.</param>
public class DateTimeValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    string? format = null,
    IFormatProvider? formatProvider = null,
    DateTimeStyles dateTimeStyles = DateTimeStyles.None
) : ValidatorBase<DateTime>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid DateTime matching the expected format; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out DateTime value, out string? errorMessage)
    {
        if (DateTime.TryParseExact(input, format, formatProvider, dateTimeStyles, out value))
        {
            errorMessage = null;
            return true;
        }

        value = default;
        errorMessage = format is null
            ? "Please enter a valid date and time."
            : $"Please enter a valid date and time in the format '{format}'.";
        return false;
    }
}
