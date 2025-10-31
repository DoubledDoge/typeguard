namespace TypeGuard.Core.Rules;

public class CustomRule<T>(Func<T, bool> predicate, string errorMessage) : IValidationRule<T>
{
	public bool IsValid(T value) => predicate(value);

	public string ErrorMessage { get; } = errorMessage;
}