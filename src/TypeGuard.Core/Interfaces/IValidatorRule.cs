namespace TypeGuard.Core.Interfaces;

/// <summary>
/// Defines a contract for a validation rule that can be applied to values of a specific type.
/// </summary>
/// <typeparam name="T">The type of value this rule validates.</typeparam>
public interface IValidatorRule<in T>
{
	/// <summary>
	/// Determines whether the specified value passes this validation rule.
	/// </summary>
	/// <param name="value">The value to validate.</param>
	/// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
	bool IsValid(T value);

	/// <summary>
	/// Gets the error message displayed when validation fails.
	/// </summary>
	string ErrorMessage { get; }
}
