namespace TypeGuard.Core.Validators;

using Abstractions;
using Rules;

/// <summary>
/// Provides a base implementation for validators that prompt users for input, parse it, and validate it against configured rules.
/// </summary>
/// <typeparam name="T">The type of value that this validator produces after successful parsing and validation. (Generic Type)</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public abstract class ValidatorBase<T>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : IValidator<T>
{
    private readonly List<IValidationRule<T>> _rules = [];

    /// <summary>
    /// Adds a validation rule to be applied to the input.
    /// </summary>
    /// <param name="rule">The validation rule to add.</param>
    /// <returns>The current validator instance for method chaining.</returns>
    public IValidator<T> AddRule(IValidationRule<T> rule)
    {
        _rules.Add(rule);
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input, validates it against all registered rules, and returns the valid input.
    /// Continues prompting until valid input is provided.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated input of type <typeparamref name="T"/>.</returns>
    public async Task<T> GetValidInputAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await outputProvider.DisplayPromptAsync(prompt, cancellationToken);
            string? rawInput = await inputProvider.GetInputAsync(cancellationToken);

            if (!TryParse(rawInput, out T? value, out string? parseError))
            {
                await outputProvider.DisplayErrorAsync(parseError!, cancellationToken);
                continue;
            }

            // Validate the input against all rules
            bool allRulesPassed = true;
            foreach (IValidationRule<T> rule in _rules.Where(rule => !rule.IsValid(value!)))
            {
                await outputProvider.DisplayErrorAsync(rule.errorMessage, cancellationToken);
                allRulesPassed = false;
                break;
            }

            if (allRulesPassed)
                return value!;
        }
    }

    /// <summary>
    /// Synchronously prompts the user for input, validates it against all registered rules, and returns the valid input.
    /// Continues prompting until valid input is provided.
    /// </summary>
    /// <returns>The validated input of type <typeparamref name="T"/>.</returns>
    public T GetValidInput() => GetValidInputAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Attempts to parse the raw user input into the target type.
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed value if parsing succeeded, or the default value if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    protected abstract bool TryParse(string? input, out T? value, out string? errorMessage);
}
