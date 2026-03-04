namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring an enum input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="EnumHandler{TEnum}"/>.
/// Defaults to constructing a standard <see cref="EnumHandler{TEnum}"/> from the provided providers.
/// </param>
public class EnumInputBuilder<TEnum>(
    string prompt,
    bool ignoreCase,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, bool, IInputProvider, IOutputProvider, EnumHandler<TEnum>>? validatorFactory = null
)
    : BuilderBase<TEnum, EnumInputBuilder<TEnum>>(
        (validatorFactory ?? ((p, ig, i, o) => new EnumHandler<TEnum>(i, o, p, ig)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            ignoreCase,
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
    where TEnum : struct, Enum
{
    /// <summary>
    /// Adds a validation rule that ensures the enum value is defined.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithDefined(string? customMessage = null) =>
        this.AddRule(new DefinedEnumRule<TEnum>(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the enum value is not the default value.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithNotDefault(string? customMessage = null) =>
        this.AddRule(new NotDefaultEnumRule<TEnum>(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the enum value is one of the allowed values.
    /// </summary>
    /// <param name="allowedValues">The collection of allowed enum values. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedValues"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="allowedValues"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithAllowedValues(
        IEnumerable<TEnum> allowedValues,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(allowedValues);

        IEnumerable<TEnum> enumerable = allowedValues.ToList();
        return !enumerable.Any()
            ? throw new ArgumentException("Cannot be empty.", nameof(allowedValues))
            : this.AddRule(new AllowedEnumValuesRule<TEnum>(enumerable, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value is not one of the excluded values.
    /// </summary>
    /// <param name="excludedValues">The collection of excluded enum values. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedValues"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="excludedValues"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithExcludedValues(
        IEnumerable<TEnum> excludedValues,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(excludedValues);

        IEnumerable<TEnum> enumerable = excludedValues.ToList();
        return !enumerable.Any()
            ? throw new ArgumentException("Cannot be empty.", nameof(excludedValues))
            : this.AddRule(new ExcludedEnumValuesRule<TEnum>(enumerable, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the enum value has the specified flag set.
    /// </summary>
    /// <param name="requiredFlag">The flag that must be set.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithHasFlag(TEnum requiredFlag, string? customMessage = null) =>
        this.AddRule(new HasFlagRule<TEnum>(requiredFlag, customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the enum value does not have the specified flag set.
    /// </summary>
    /// <param name="forbiddenFlag">The flag that must not be set.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithNotHasFlag(
        TEnum forbiddenFlag,
        string? customMessage = null
    ) => this.AddRule(new NotHasFlagRule<TEnum>(forbiddenFlag, customMessage));

    /// <summary>
    /// Adds a custom validation rule to the handler.
    /// </summary>
    /// <param name="predicate">The function that determines whether an enum value is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public EnumInputBuilder<TEnum> WithCustomRule(Func<TEnum, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<TEnum>(predicate, errorMessage));
    }
}
