namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// A validator that prompts for and validates single character input.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class CharValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<char>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is exactly one character; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out char value, out string? errorMessage)
    {
        if (string.IsNullOrEmpty(input) || input.Length != 1)
        {
            value = default;
            errorMessage = "Please enter a single character.";
            return false;
        }

        value = input[0];
        errorMessage = null;
        return true;
    }
}
