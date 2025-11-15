namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates GUID (Globally Unique Identifier) input.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class GuidValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<Guid>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into a GUID. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed GUID if parsing succeeded, or <see cref="Guid.Empty"/> if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid GUID, otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out Guid value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a valid GUID";
            value = Guid.Empty;
            return false;
        }

        if (Guid.TryParse(input, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = "Please enter a valid GUID (e.g., 12345678-1234-1234-1234-123456789abc)";
        value = Guid.Empty;
        return false;
    }
}
