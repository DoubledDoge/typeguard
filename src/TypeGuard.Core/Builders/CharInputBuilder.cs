namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a character input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="CharHandler"/>.
/// Defaults to constructing a standard <see cref="CharHandler"/> from the provided providers.
/// </param>
public class CharInputBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, IInputProvider, IOutputProvider, CharHandler>? validatorFactory = null
)
    : BuilderBase<char, CharInputBuilder>(
        (validatorFactory ?? ((p, i, o) => new CharHandler(i, o, p)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a validation rule that ensures the character is a letter.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithLetter(string? customMessage = null) =>
        this.AddRule(new LetterRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is a digit.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithDigit(string? customMessage = null) =>
        this.AddRule(new DigitRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is uppercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithUpperCase(string? customMessage = null) =>
        this.AddRule(new UpperCaseRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is lowercase.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithLowerCase(string? customMessage = null) =>
        this.AddRule(new LowerCaseRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is alphanumeric (letter or digit).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithAlphanumeric(string? customMessage = null) =>
        this.AddRule(new AlphanumericCharRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is whitespace.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithWhitespace(string? customMessage = null) =>
        this.AddRule(new WhitespaceRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is a punctuation mark.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithPunctuation(string? customMessage = null) =>
        this.AddRule(new PunctuationRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the character is within a specified set of allowed characters.
    /// </summary>
    /// <param name="allowedChars">The string containing all allowed characters. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="allowedChars"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithAllowedChars(string allowedChars, string? customMessage = null)
    {
        return string.IsNullOrEmpty(allowedChars)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(allowedChars))
            : this.AddRule(new AllowedCharsRule(allowedChars, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the character is not within a specified set of excluded characters.
    /// </summary>
    /// <param name="excludedChars">The string containing all excluded characters. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="excludedChars"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithExcludedChars(string excludedChars, string? customMessage = null)
    {
        return string.IsNullOrEmpty(excludedChars)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(excludedChars))
            : this.AddRule(new ExcludedCharsRule(excludedChars, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the character falls within the specified range.
    /// </summary>
    /// <param name="min">The minimum acceptable character (inclusive).</param>
    /// <param name="max">The maximum acceptable character (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithRange(char min, char max, string? customMessage = null)
    {
        return min > max
            ? throw new ArgumentException(
                $"min ('{min}') must be less than or equal to max ('{max}').",
                nameof(min)
            )
            : this.AddRule(new RangeRule<char>(min, max, customMessage));
    }

    /// <summary>
    /// Adds a custom validation rule to the handler.
    /// </summary>
    /// <param name="predicate">The function that determines whether a character is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public CharInputBuilder WithCustomRule(Func<char, bool> predicate, string errorMessage)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<char>(predicate, errorMessage));
    }
}
