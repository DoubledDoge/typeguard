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
public class StringBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly StringValidator _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a rule that ensures that the string length is within the specified range.
    /// </summary>
    /// <param name="minLength">The minimum optional acceptable length.</param>
    /// <param name="maxLength">The maximum optional acceptable length.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithLengthRange(
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
    public StringBuilder WithNoDigits(string? customMessage = null)
    {
        _validator.AddRule(new NumericRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a regular expression validation rule to the validator.
    /// </summary>
    /// <param name="pattern">The regular expression pattern that the string must match.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithRegex(string pattern, string? customMessage = null)
    {
        _validator.AddRule(new RegexRule(pattern, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string contains only alphabetic characters.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithAlphabetic(string? customMessage = null)
    {
        _validator.AddRule(new AlphabeticRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string contains only alphabetic or numeric characters.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithAlphanumeric(string? customMessage = null)
    {
        _validator.AddRule(new AlphanumericStringRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string is in uppercase.
    /// </summary>
    /// <param name="customMessage"></param>
    /// <returns></returns>
    public StringBuilder WithUpperCase(string? customMessage = null)
    {
        _validator.AddRule(new UpperCaseStringRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string is in lowercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithLowerCase(string? customMessage = null)
    {
        _validator.AddRule(new LowerCaseStringRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string starts with the specified prefix.
    /// </summary>
    /// <param name="prefix">A character or group of characters that the string must start with.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>-
    public StringBuilder WithStart(string prefix, string? customMessage = null)
    {
        _validator.AddRule(new StartsWithRule(prefix, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string ends with the specified suffix.
    /// </summary>
    /// <param name="suffix">A character or group of characters that the string must end with.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithEnd(string suffix, string? customMessage = null)
    {
        _validator.AddRule(new EndsWithRule(suffix, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string contains the specified substring.
    /// </summary>
    /// <param name="substring">A character or group of characters that the string must contain.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithContains(string substring, string? customMessage = null)
    {
        _validator.AddRule(new ContainsRule(substring, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string does not contain the specified substring.
    /// </summary>
    /// <param name="substring">A character or group of characters that the string must not contain.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithNotContains(string substring, string? customMessage = null)
    {
        _validator.AddRule(new NotContainsRule(substring, customMessage));
        return this;
    }


    /// <summary>
    /// Adds a rule that ensures the string contains values from a specified set.
    /// </summary>
    /// <param name="allowedValues">An array of string values that the string must contain.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithAllowedValues(string[] allowedValues, string? customMessage = null)
    {
        _validator.AddRule(new AllowedValuesRule(allowedValues, customMessage));
        return this;
    }


    /// <summary>
    /// Adds a rule that ensures the string does not contain values from a specified set.
    /// </summary>
    /// <param name="excludedValues">An array of string values that the string must not contain.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithExcludedValues(string[] excludedValues, string? customMessage = null)
    {
        _validator.AddRule(new ExcludedValuesRule(excludedValues, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string is in a valid email format.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithEmailFormat(string? customMessage = null)
    {
        _validator.AddRule(new EmailRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string is in a valid phone format.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithPhoneFormat(string? customMessage = null)
    {
        _validator.AddRule(new PhoneRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a rule that ensures the string is in a valid file path format.
    /// </summary>
    /// <param name="mustExist">A boolean that determines weather the file must exist or not.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithFilePathFormat(bool mustExist = false, string? customMessage = null)
    {
        _validator.AddRule(new FilePathRule(mustExist, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a string value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public StringBuilder WithCustomRule(Func<string, bool> predicate, string errorMessage)
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
