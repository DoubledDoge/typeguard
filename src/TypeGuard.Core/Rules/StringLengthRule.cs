namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a string's length falls within specified minimum and maximum bounds.
/// </summary>
public class StringLengthRule : IValidationRule<string>
{
	private readonly int? _minLength;
	private readonly int? _maxLength;

	/// <summary>
	/// Initializes a new instance of the <see cref="StringLengthRule"/> class with the specified length constraints.
	/// </summary>
	/// <param name="minLength">The minimum acceptable length.</param>
	/// <param name="maxLength">The maximum acceptable length.</param>
	/// <param name="customMessage">An optional custom error message. If not provided, a default message based on the constraints is used.</param>
	public StringLengthRule(int? minLength = null, int? maxLength = null, string? customMessage = null)
	{
		_minLength = minLength;
		_maxLength = maxLength;

		if (customMessage != null)
			errorMessage = customMessage;
		else if (minLength.HasValue && maxLength.HasValue)
			errorMessage = $"Length must be between {minLength} and {maxLength} characters";
		else if (minLength.HasValue)
			errorMessage = $"Length must be at least {minLength} characters";
		else if (maxLength.HasValue)
			errorMessage = $"Length must be at most {maxLength} characters";
		else
			errorMessage = "Invalid length";
	}

	/// <summary>
	/// Determines whether the specified string's length falls within the configured bounds.
	/// </summary>
	/// <param name="value">The string value to validate.</param>
	/// <returns><c>true</c> if the string length meets the minimum and maximum requirements; otherwise, <c>false</c>.</returns>
	public bool IsValid(string value)
	{
		int length = value.Length;

		if (_minLength.HasValue && length < _minLength.Value)
			return false;

		return !_maxLength.HasValue || length <= _maxLength.Value;
	}

	/// <summary>
	/// Gets the error message that should be displayed when validation fails.
	/// </summary>
	public string errorMessage { get; }
}
