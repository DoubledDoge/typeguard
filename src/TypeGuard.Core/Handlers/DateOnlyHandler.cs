using System.Globalization;

namespace TypeGuard.Core.Handlers;

using Interfaces;

/// <summary>
/// An input handler that prompts for and validates DateOnly input according to a specified format.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
/// <param name="formatProvider">The format provider to use for parsing. Defaults to the current culture.</param>
/// <param name="dateTimeStyles">The styles to use for parsing. Defaults to <see cref="DateTimeStyles.None"/>.</param>
public class DateOnlyHandler(
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	string prompt,
	string? format = null,
	IFormatProvider? formatProvider = null,
	DateTimeStyles dateTimeStyles = DateTimeStyles.None
) : HandlerBase<DateOnly>(inputProvider, outputProvider, prompt)
{
	/// <inheritdoc cref="HandlerBase{T}.TryParse"/>
	/// <returns><c>true</c> if the input is a valid DateOnly matching the expected format; otherwise, <c>false</c>.</returns>
	protected override bool TryParse(string? input, out DateOnly value, out string? errorMessage)
	{
		if (DateOnly.TryParseExact(input, format, formatProvider, dateTimeStyles, out value))
		{
			errorMessage = null;
			return true;
		}

		value = default;
		errorMessage = format is null
			? "Please enter a valid date."
			: $"Please enter a valid date in the format '{format}'.";
		return false;
	}
}
