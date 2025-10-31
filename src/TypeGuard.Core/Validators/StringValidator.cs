namespace TypeGuard.Core.Validators;

using Abstractions;

public class StringValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<string>(inputProvider, outputProvider, prompt)
{
    protected override bool TryParse(string? input, out string? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Input cannot be empty or whitespace";
            value = null;
            return false;
        }

        value = input.Trim();
        errorMessage = null;
        return true;
    }
}
