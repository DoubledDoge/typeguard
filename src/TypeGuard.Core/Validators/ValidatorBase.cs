namespace TypeGuard.Core.Validators;

using Abstractions;
using Rules;

public abstract class ValidatorBase<T>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : IValidator<T>
{
    private readonly List<IValidationRule<T>> _rules = [];

    public IValidator<T> AddRule(IValidationRule<T> rule)
    {
        _rules.Add(rule);
        return this;
    }

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
                await outputProvider.DisplayErrorAsync(rule.ErrorMessage, cancellationToken);
                allRulesPassed = false;
                break;
            }

            if (allRulesPassed)
                return value!;
        }
    }

    public T GetValidInput() => GetValidInputAsync().GetAwaiter().GetResult();

    protected abstract bool TryParse(string? input, out T? value, out string? errorMessage);
}
