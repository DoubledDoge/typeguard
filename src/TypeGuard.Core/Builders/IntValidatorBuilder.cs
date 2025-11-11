namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring an integer validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class IntValidatorBuilder(string prompt, IInputProvider inputProvider, IOutputProvider outputProvider)
{
	private readonly IntValidator _validator = new(inputProvider, outputProvider, prompt);

	/// <summary>
	/// Adds a range validation rule to the validator.
	/// </summary>
	/// <param name="min">The minimum acceptable value.</param>
	/// <param name="max">The maximum acceptable value.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public IntValidatorBuilder WithRange(int min, int max, string? customMessage = null)
	{
		_validator.AddRule(new RangeRule<int>(min, max, customMessage));
		return this;
	}

	/// <summary>
	/// Adds a custom validation rule to the validator.
	/// </summary>
	/// <param name="predicate">The function that determines whether an integer value is valid.</param>
	/// <param name="errorMessage">The error message to display when validation fails.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	public IntValidatorBuilder WithCustomRule(Func<int, bool> predicate, string errorMessage)
	{
		_validator.AddRule(new CustomRule<int>(predicate, errorMessage));
		return this;
	}

	/// <summary>
	/// Asynchronously prompts the user for input and returns the validated integer.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the validated integer.</returns>
	public async Task<int> GetAsync(CancellationToken cancellationToken = default) => await _validator.GetValidInputAsync(cancellationToken);

	/// <summary>
	/// Synchronously prompts the user for input and returns the validated integer.
	/// </summary>
	/// <returns>The validated integer.</returns>
	public int Get() => _validator.GetValidInput();
}
