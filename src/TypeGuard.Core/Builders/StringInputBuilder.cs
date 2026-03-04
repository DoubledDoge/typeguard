namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a string input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="StringHandler"/>.
/// Defaults to constructing a standard <see cref="StringHandler"/> from the provided providers.
/// </param>
public class StringInputBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, IInputProvider, IOutputProvider, StringHandler>? validatorFactory = null
)
    : BuilderBase<string, StringInputBuilder>(
        (validatorFactory ?? ((p, i, o) => new StringHandler(i, o, p)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a rule that ensures the string length is within the specified range.
    /// </summary>
    /// <param name="minLength">The optional minimum acceptable length (inclusive). Must be greater than or equal to zero.</param>
    /// <param name="maxLength">The optional maximum acceptable length (inclusive). Must be greater than or equal to zero.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minLength"/> or <paramref name="maxLength"/> is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="minLength"/> is greater than <paramref name="maxLength"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithLengthRange(
        int? minLength = null,
        int? maxLength = null,
        string? customMessage = null
    )
    {
        if (minLength is < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(minLength),
                minLength,
                "minLength must be greater than or equal to zero."
            );
        }

        if (maxLength is < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxLength),
                maxLength,
                "maxLength must be greater than or equal to zero."
            );
        }

        if (minLength.HasValue && maxLength.HasValue && minLength.Value > maxLength.Value)
        {
            throw new ArgumentException(
                $"minLength ({minLength}) must be less than or equal to maxLength ({maxLength}).",
                nameof(minLength)
            );
        }

        return this.AddRule(new StringLengthRule(minLength, maxLength, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string contains numeric digits.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithDigits(string? customMessage = null) =>
        this.AddRule(new NumericRule(customMessage));

    /// <summary>
    /// Adds a regular expression validation rule to the validator.
    /// </summary>
    /// <param name="pattern">The regular expression pattern the string must match. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithRegex(string pattern, string? customMessage = null)
    {
        return string.IsNullOrEmpty(pattern)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(pattern))
            : this.AddRule(new RegexRule(pattern, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string contains only alphabetic characters.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithAlphabetic(string? customMessage = null) =>
        this.AddRule(new AlphabeticRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string contains only alphabetic or numeric characters.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithAlphanumeric(string? customMessage = null) =>
        this.AddRule(new AlphanumericStringRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string is entirely uppercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithUpperCase(string? customMessage = null) =>
        this.AddRule(new UpperCaseStringRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string is entirely lowercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithLowerCase(string? customMessage = null) =>
        this.AddRule(new LowerCaseStringRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string starts with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix the string must start with. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="prefix"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithStart(string prefix, string? customMessage = null)
    {
        return string.IsNullOrEmpty(prefix)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(prefix))
            : this.AddRule(new StartsWithRule(prefix, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string ends with the specified suffix.
    /// </summary>
    /// <param name="suffix">The suffix the string must end with. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="suffix"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithEnd(string suffix, string? customMessage = null)
    {
        return string.IsNullOrEmpty(suffix)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(suffix))
            : this.AddRule(new EndsWithRule(suffix, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string contains the specified substring.
    /// </summary>
    /// <param name="substring">The substring the string must contain. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="substring"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithContains(string substring, string? customMessage = null)
    {
        return string.IsNullOrEmpty(substring)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(substring))
            : this.AddRule(new ContainsRule(substring, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string does not contain the specified substring.
    /// </summary>
    /// <param name="substring">The substring the string must not contain. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="substring"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithNotContains(string substring, string? customMessage = null)
    {
        return string.IsNullOrEmpty(substring)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(substring))
            : this.AddRule(new NotContainsRule(substring, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string is one of the specified allowed values.
    /// </summary>
    /// <param name="allowedValues">The array of allowed string values. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedValues"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="allowedValues"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithAllowedValues(
        string[] allowedValues,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(allowedValues);

        return allowedValues.Length == 0
            ? throw new ArgumentException("Cannot be empty.", nameof(allowedValues))
            : this.AddRule(new AllowedValuesRule(allowedValues, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string is not one of the specified excluded values.
    /// </summary>
    /// <param name="excludedValues">The array of excluded string values. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedValues"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="excludedValues"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithExcludedValues(
        string[] excludedValues,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(excludedValues);

        return excludedValues.Length == 0
            ? throw new ArgumentException("Cannot be empty.", nameof(excludedValues))
            : this.AddRule(new ExcludedValuesRule(excludedValues, customMessage));
    }

    /// <summary>
    /// Adds a rule that ensures the string is in a valid email format.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithEmailFormat(string? customMessage = null) =>
        this.AddRule(new EmailRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string is in a valid phone number format.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithPhoneFormat(string? customMessage = null) =>
        this.AddRule(new PhoneRule(customMessage));

    /// <summary>
    /// Adds a rule that ensures the string is a valid file path format.
    /// </summary>
    /// <param name="mustExist">Whether the file at the specified path must exist on disk. Defaults to false.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithFilePathFormat(
        bool mustExist = false,
        string? customMessage = null
    ) => this.AddRule(new FilePathRule(mustExist, customMessage));

    /// <summary>
    /// Adds a custom validation rule to the input handler.
    /// </summary>
    /// <param name="predicate">The function that determines whether a string value is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public StringInputBuilder WithCustomRule(Func<string, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<string>(predicate, errorMessage));
    }
}
