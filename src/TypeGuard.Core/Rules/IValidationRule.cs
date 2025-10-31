namespace TypeGuard.Core.Rules;

public interface IValidationRule<in T>
{
	bool   IsValid(T value);
	string ErrorMessage { get; }
}