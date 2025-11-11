namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a string validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class StringValidatorBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly StringValidator _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a length validation rule to the validator.
    /// </summary>
    /// <param name="minLength">The minimum optional acceptable length.</param>
    /// <param name="maxLength">The maximum optional acceptable length.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringValidatorBuilder WithLengthRange(
        int? minLength = null,
        int? maxLength = null,
        string? customMessage = null
    )
    {
        _validator.AddRule(new StringLengthRule(minLength, maxLength, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string contains no numeric digits.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringValidatorBuilder WithNoDigits(string? customMessage = null)
    {
        _validator.AddRule(new NoDigitsRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a regular expression validation rule to the validator.
    /// </summary>
    /// <param name="pattern">The regular expression pattern that the string must match.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringValidatorBuilder WithRegex(string pattern, string? customMessage = null)
    {
        _validator.AddRule(new RegexRule(pattern, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a string value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringValidatorBuilder WithCustomRule(Func<string, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<string>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated string.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated string.</returns>
    public async Task<string> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated string.
    /// </summary>
    /// <returns>The validated string.</returns>
    public string Get() => _validator.GetValidInput();
}
