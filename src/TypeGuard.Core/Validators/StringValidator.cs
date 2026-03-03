namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// A validator that prompts for and validates string input, ensuring it is not empty or whitespace.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class StringValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<string>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is not null, empty, or whitespace; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out string? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = null;
            errorMessage = "Input cannot be empty or whitespace.";
            return false;
        }

        value = input.Trim();
        errorMessage = null;
        return true;
    }
}
