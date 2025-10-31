namespace TypeGuard.Core.Rules;

public class StringLengthRule : IValidationRule<string>
{
	private readonly int? _minLength;
	private readonly int? _maxLength;
    
	public StringLengthRule(int? minLength = null, int? maxLength = null, string? customMessage = null)
	{
		_minLength = minLength;
		_maxLength = maxLength;
        
		if (customMessage != null)
			ErrorMessage = customMessage;
		else if (minLength.HasValue && maxLength.HasValue)
			ErrorMessage = $"Length must be between {minLength} and {maxLength} characters";
		else if (minLength.HasValue)
			ErrorMessage = $"Length must be at least {minLength} characters";
		else if (maxLength.HasValue)
			ErrorMessage = $"Length must be at most {maxLength} characters";
		else
			ErrorMessage = "Invalid length";
	}
    
	public bool IsValid(string value)
	{
		int length = value.Length;
        
		if (_minLength.HasValue && length < _minLength.Value)
			return false;
            
		return !_maxLength.HasValue || length <= _maxLength.Value;
	}
    
	public string ErrorMessage { get; }
}