namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring an enum validator with validation rules.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Default is true.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class EnumBuilder<TEnum>(
    string prompt,
    bool ignoreCase,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
) where TEnum : struct, Enum
{
    private readonly EnumValidator<TEnum> _validator = new(inputProvider, outputProvider, prompt, ignoreCase);

    /// <summary>
    /// Adds a validation rule that ensures the enum value is defined.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithDefined(string? customMessage = null)
    {
        _validator.AddRule(new DefinedEnumRule<TEnum>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value is not the default value.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithNotDefault(string? customMessage = null)
    {
        _validator.AddRule(new NotDefaultEnumRule<TEnum>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value is one of the allowed values.
    /// </summary>
    /// <param name="allowedValues">The collection of allowed enum values.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithAllowedValues(IEnumerable<TEnum> allowedValues, string? customMessage = null)
    {
        _validator.AddRule(new AllowedEnumValuesRule<TEnum>(allowedValues, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value is not one of the excluded values.
    /// </summary>
    /// <param name="excludedValues">The collection of excluded enum values.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithExcludedValues(IEnumerable<TEnum> excludedValues, string? customMessage = null)
    {
        _validator.AddRule(new ExcludedEnumValuesRule<TEnum>(excludedValues, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value has a specific flag set.
    /// </summary>
    /// <param name="requiredFlag">The flag that must be set.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithHasFlag(TEnum requiredFlag, string? customMessage = null)
    {
        _validator.AddRule(new HasFlagRule<TEnum>(requiredFlag, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value does not have a specific flag set.
    /// </summary>
    /// <param name="forbiddenFlag">The flag that must not be set.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithNotHasFlag(TEnum forbiddenFlag, string? customMessage = null)
    {
        _validator.AddRule(new NotHasFlagRule<TEnum>(forbiddenFlag, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether an enum value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public EnumBuilder<TEnum> WithCustomRule(Func<TEnum, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<TEnum>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated enum value.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated enum value.</returns>
    public async Task<TEnum> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated enum value.
    /// </summary>
    /// <returns>The validated enum value.</returns>
    public TEnum Get() => _validator.GetValidInput();
}
