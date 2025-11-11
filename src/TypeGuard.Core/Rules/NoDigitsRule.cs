namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a string does not contain any numeric digits.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NoDigitsRule(string? customMessage = null) : IValidationRule<string>
{
	/// <summary>
	/// Determines whether the specified string contains no numeric digits.
	/// </summary>
	/// <param name="value">The string value to validate.</param>
	/// <returns><c>true</c> if the string contains no digits; otherwise, <c>false</c>.</returns>
	public bool IsValid(string value) => !value.Any(char.IsDigit);

	/// <summary>
	/// Gets the error message that should be displayed when validation fails.
	/// </summary>
	public string errorMessage { get; } = customMessage ?? "Input cannot contain digits";
}
