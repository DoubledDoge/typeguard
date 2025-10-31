namespace TypeGuard.Core.Validators;

using Abstractions;

public class IntValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<int>(inputProvider, outputProvider, prompt)
{
    protected override bool TryParse(string? input, out int value, out string? errorMessage)
    {
        if (int.TryParse(input, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = "Please enter a valid integer";
        value = 0;
        return false;
    }
}
