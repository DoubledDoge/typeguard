namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// Provides a base implementation for validators that prompt users for input, parse it,
/// and validate it against configured rules.
/// </summary>
/// <typeparam name="T">The type of value this validator produces after successful parsing and validation.</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="inputProvider"/>, <paramref name="outputProvider"/>, or <paramref name="prompt"/> is null.</exception>
public abstract class ValidatorBase<T>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : IValidator<T>
{
    private readonly IInputProvider _inputProvider =
        inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));

    private readonly IOutputProvider _outputProvider =
        outputProvider ?? throw new ArgumentNullException(nameof(outputProvider));

    private readonly string _prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));

    private readonly List<IValidationRule<T>> _rules = [];

    /// <inheritdoc/>
    public IValidator<T> AddRule(IValidationRule<T> rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        _rules.Add(rule);
        return this;
    }

    /// <inheritdoc/>
    public async Task<T> GetValidInputAsync(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _outputProvider.DisplayPromptAsync(_prompt, cancellationToken);
            string? rawInput = await _inputProvider.GetInputAsync(cancellationToken);

            if (!TryParse(rawInput, out T? value, out string? parseError))
            {
                await _outputProvider.DisplayErrorAsync(parseError!, cancellationToken);
                continue;
            }

            bool allRulesPassed = true;
            foreach (IValidationRule<T> rule in _rules.Where(rule => !rule.IsValid(value!)))
            {
                await _outputProvider.DisplayErrorAsync(rule.ErrorMessage, cancellationToken);
                allRulesPassed = false;
                break;
            }

            if (allRulesPassed)
            {
                return value!;
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method blocks the calling thread by running <see cref="GetValidInputAsync"/> synchronously.
    /// Avoid calling this from a context that has a synchronization context (such as ASP.NET or UI threads),
    /// as it may cause a deadlock. Prefer <see cref="GetValidInputAsync"/> in async contexts.
    /// </remarks>
    public T GetValidInput() => GetValidInputAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Attempts to parse the raw user input string into the target type.
    /// </summary>
    /// <param name="input">The raw input string from the user. May be null if no input was provided.</param>
    /// <param name="value">When this method returns, contains the parsed value if parsing succeeded, or the default value if it failed.</param>
    /// <param name="errorMessage">When this method returns, contains an error message if parsing failed, or null if it succeeded.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    protected abstract bool TryParse(string? input, out T? value, out string? errorMessage);
}
