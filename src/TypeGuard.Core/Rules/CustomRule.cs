namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that uses a custom predicate function to determine validity.
/// </summary>
/// <typeparam name="T">The type of value this rule validates.</typeparam>
/// <param name="predicate">The function that determines whether a value is valid. Cannot be null.</param>
/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
public class CustomRule<T>(Func<T, bool> predicate, string errorMessage) : IValidatorRule<T>
{
	private readonly Func<T, bool> _predicate =
		predicate ?? throw new ArgumentNullException(nameof(predicate));

	/// <inheritdoc/>
	public bool IsValid(T value) => _predicate(value);

	/// <inheritdoc/>
	public string ErrorMessage { get; } =
		errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
}
