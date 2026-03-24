namespace TypeGuard.Core.Interfaces;

/// <summary>
/// Defines a contract for handling user input against a set of rules and retrieving valid input of a specific type.
/// </summary>
/// <typeparam name="T">The type of the validated input value.</typeparam>
public interface IHandler<T>
{
	/// <summary>
	/// Adds a validation rule to be applied to the input.
	/// </summary>
	/// <param name="rule">The validation rule to add.</param>
	/// <exception cref="ArgumentNullException">Thrown when rule is null.</exception>
	/// <returns>The current validator instance for method chaining.</returns>
	IHandler<T> AddRule(IValidatorRule<T> rule);

	/// <summary>
	/// Asynchronously prompts the user for input, validates it against all registered rules, and returns the handled input.
	/// Continues prompting until valid input is provided.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the handled input of type <typeparamref name="T"/>.</returns>
	Task<T> GetValidInputAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Synchronously prompts the user for input, validates it against all registered rules, and returns the handled input.
	/// Continues prompting until valid input is provided.
	/// </summary>
	/// <returns>The handled input of type <typeparamref name="T"/>.</returns>
	T GetValidInput();
}
