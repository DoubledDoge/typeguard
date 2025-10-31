using System.Globalization;


namespace TypeGuard.Core.Validators;
using Abstractions;

public class DateTimeValidator(
	IInputProvider   inputProvider,
	IOutputProvider  outputProvider,
	string           prompt,
	string?          format         = null,
	IFormatProvider? formatProvider = null,
	DateTimeStyles   dateTimeStyles = DateTimeStyles.None)
	: ValidatorBase<DateTime>(inputProvider, outputProvider, prompt)
{
	protected override bool TryParse(string? input, out DateTime value, out string? errorMessage)
	{
		if (DateTime.TryParseExact(
			    input,
			    format,
			    formatProvider,
			    dateTimeStyles,
			    out value))
		{
			errorMessage = null;
			return true;
		}

		errorMessage = $"Please enter a valid date in the format {format}";
		value = default(DateTime);
		return false;
	}
}