namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates integer input.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class IntValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<int>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into an integer. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed integer if parsing succeeded, or 0 if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid integer; otherwise, <c>false</c>.</returns>
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
