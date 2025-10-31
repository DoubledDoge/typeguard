namespace TypeGuard.Core.Rules;

public class NoDigitsRule(string? customMessage = null) : IValidationRule<string>
{
	public bool IsValid(string value) => !value.Any(char.IsDigit);

	public string ErrorMessage { get; } = customMessage ?? "Input cannot contain digits";
}