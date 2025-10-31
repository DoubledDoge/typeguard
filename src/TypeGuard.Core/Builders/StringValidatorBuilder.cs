namespace TypeGuard.Core.Builders;
using Abstractions;
using Validators;
using Rules;

public class StringValidatorBuilder(string prompt, IInputProvider inputProvider, IOutputProvider outputProvider)
{
	private readonly StringValidator _validator = new(inputProvider, outputProvider, prompt);

	public StringValidatorBuilder WithLengthRange(int? minLength = null, int? maxLength = null, string? customMessage = null)
	{
		_validator.AddRule(new StringLengthRule(minLength, maxLength, customMessage));
		return this;
	}

	public StringValidatorBuilder WithNoDigits(string? customMessage = null)
	{
		_validator.AddRule(new NoDigitsRule(customMessage));
		return this;
	}

	public StringValidatorBuilder WithRegex(string pattern, string? customMessage = null)
	{
		_validator.AddRule(new RegexRule(pattern, customMessage));
		return this;
	}

	public StringValidatorBuilder WithCustomRule(Func<string, bool> predicate, string errorMessage)
	{
		_validator.AddRule(new CustomRule<string>(predicate, errorMessage));
		return this;
	}

	public async Task<string> GetAsync(CancellationToken cancellationToken = default) => await _validator.GetValidInputAsync(cancellationToken);

	public string Get() => _validator.GetValidInput();
}