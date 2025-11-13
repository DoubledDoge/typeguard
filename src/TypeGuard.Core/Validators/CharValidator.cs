namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates single character input.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class CharValidator(
    IInputProvider  inputProvider,
    IOutputProvider outputProvider,
    string          prompt
) : ValidatorBase<char>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into a single character. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed character if parsing succeeded, or the null character if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is exactly one character; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out char value, out string? errorMessage)
    {
        if (string.IsNullOrEmpty(input) || input.Length != 1)
        {
            errorMessage = "Please enter a single character";
            value = '\0';
            return false;
        }

        value = input[0];
        errorMessage = null;
        return true;
    }
}
