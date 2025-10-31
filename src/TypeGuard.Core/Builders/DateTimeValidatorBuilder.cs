namespace TypeGuard.Core.Builders;
using Abstractions;
using Validators;
using Rules;

public class DateTimeValidatorBuilder(
	string          prompt,
	string?         format,
	IInputProvider  inputProvider,
	IOutputProvider outputProvider)
{
	private readonly DateTimeValidator _validator = new(inputProvider, outputProvider, prompt, format);

	public DateTimeValidatorBuilder WithRange(DateTime min, DateTime max, string? customMessage = null)
	{
		_validator.AddRule(new RangeRule<DateTime>(min, max, customMessage));
		return this;
	}

	public DateTimeValidatorBuilder WithCustomRule(Func<DateTime, bool> predicate, string errorMessage)
	{
		_validator.AddRule(new CustomRule<DateTime>(predicate, errorMessage));
		return this;
	}

	public async Task<DateTime> GetAsync(CancellationToken cancellationToken = default) => await _validator.GetValidInputAsync(cancellationToken);

	public DateTime Get() => _validator.GetValidInput();
}