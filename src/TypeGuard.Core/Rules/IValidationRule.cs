namespace TypeGuard.Core.Rules;

/// <summary>
/// Defines a contract for a validation rule that can be applied to values of a specific type.
/// </summary>
/// <typeparam name="T">The type of value that this rule can validate. (Generic Type)</typeparam>
public interface IValidationRule<in T>
{
	/// <summary>
	/// Determines whether the specified value passes this validation rule.
	/// </summary>
	/// <param name="value">The value to validate.</param>
	/// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
	bool   IsValid(T value);

	/// <summary>
	/// Gets the error message that should be displayed when validation fails.
	/// </summary>
	string errorMessage { get; }
}
