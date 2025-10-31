namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

public class IntValidatorBuilder(string prompt, IInputProvider inputProvider, IOutputProvider outputProvider)
{
	private readonly IntValidator _validator = new(inputProvider, outputProvider, prompt);

	public IntValidatorBuilder WithRange(int min, int max, string? customMessage = null)
	{
		_validator.AddRule(new RangeRule<int>(min, max, customMessage));
		return this;
	}

	public IntValidatorBuilder WithCustomRule(Func<int, bool> predicate, string errorMessage)
	{
		_validator.AddRule(new CustomRule<int>(predicate, errorMessage));
		return this;
	}

	public async Task<int> GetAsync(CancellationToken cancellationToken = default) => await _validator.GetValidInputAsync(cancellationToken);

	public int Get() => _validator.GetValidInput();
}