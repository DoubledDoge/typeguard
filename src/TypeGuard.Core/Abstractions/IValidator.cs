namespace TypeGuard.Core.Abstractions;

using Rules;

/// <summary>
/// Defines a contract for validating user input against a set of rules and retrieving valid input of a specific type.
/// </summary>
/// <typeparam name="T">The type of the validated input value. (Generic Type)</typeparam>
public interface IValidator<T>
{
    /// <summary>
    /// Adds a validation rule to be applied to the input.
    /// </summary>
    /// <param name="rule">The validation rule to add.</param>
    /// <returns>The current validator instance for method chaining.</returns>
    IValidator<T> AddRule(IValidationRule<T> rule);

    /// <summary>
    /// Asynchronously prompts the user for input, validates it against all registered rules, and returns the valid input.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated input of type <typeparamref name="T"/>.</returns>
    Task<T> GetValidInputAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously prompts the user for input, validates it against all registered rules, and returns the valid input.
    /// </summary>
    /// <returns>The validated input of type <typeparamref name="T"/>.</returns>
    T GetValidInput(); // Sync wrapper
}
