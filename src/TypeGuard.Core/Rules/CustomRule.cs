namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that uses a custom predicate function to determine validity.
/// </summary>
/// <typeparam name="T">The type of value that this rule can validate.</typeparam>
/// <param name="predicate">The function that determines whether a value is valid.</param>
/// <param name="errorMessage">The error message to display when validation fails. (Generic Type)</param>
public class CustomRule<T>(Func<T, bool> predicate, string errorMessage) : IValidationRule<T>
{
	/// <summary>
	/// Determines whether the specified value passes this validation rule by executing the custom predicate.
	/// </summary>
	/// <param name="value">The value to validate.</param>
	/// <returns><c>true</c> if the predicate returns true; otherwise, <c>false</c>.</returns>
	public bool IsValid(T value) => predicate(value);

	/// <summary>
	/// Gets the error message that should be displayed when validation fails.
	/// </summary>
	public string errorMessage { get; } = errorMessage;
}
