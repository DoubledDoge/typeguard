namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a character validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class CharBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly CharValidator _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a validation rule that ensures the character is a letter.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithLetter(string? customMessage = null)
    {
        _validator.AddRule(new LetterRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is a digit.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithDigit(string? customMessage = null)
    {
        _validator.AddRule(new DigitRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is uppercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithUpperCase(string? customMessage = null)
    {
        _validator.AddRule(new UpperCaseRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is lowercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithLowerCase(string? customMessage = null)
    {
        _validator.AddRule(new LowerCaseRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is alphanumeric (letter or digit).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithAlphanumeric(string? customMessage = null)
    {
        _validator.AddRule(new AlphanumericCharRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is whitespace.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithWhitespace(string? customMessage = null)
    {
        _validator.AddRule(new WhitespaceRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is a punctuation mark.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithPunctuation(string? customMessage = null)
    {
        _validator.AddRule(new PunctuationRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is within a specified set of allowed characters.
    /// </summary>
    /// <param name="allowedChars">The string containing all allowed characters.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithAllowedChars(string allowedChars, string? customMessage = null)
    {
        _validator.AddRule(new AllowedCharsRule(allowedChars, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is not within a specified set of excluded characters.
    /// </summary>
    /// <param name="excludedChars">The string containing all excluded characters.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithExcludedChars(string excludedChars, string? customMessage = null)
    {
        _validator.AddRule(new ExcludedCharsRule(excludedChars, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a range validation rule to the validator.
    /// </summary>
    /// <param name="min">The minimum acceptable character.</param>
    /// <param name="max">The maximum acceptable character.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithRange(char min, char max, string? customMessage = null)
    {
        _validator.AddRule(new RangeRule<char>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a character is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public CharBuilder WithCustomRule(Func<char, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<char>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated character.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated character.</returns>
    public async Task<char> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated character.
    /// </summary>
    /// <returns>The validated character.</returns>
    public char Get() => _validator.GetValidInput();
}
