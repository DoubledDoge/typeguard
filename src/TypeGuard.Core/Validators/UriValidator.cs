namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates URI input.
/// Accepts absolute or relative URIs depending on the specified kind.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="uriKind">The kind of URI to accept (Absolute, Relative, or RelativeOrAbsolute). Default is Absolute.</param>
public class UriValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    UriKind uriKind = UriKind.Absolute
) : ValidatorBase<Uri>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into a Uri. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed Uri if parsing succeeded, or null if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid Uri; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out Uri? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a valid URI";
            value = null;
            return false;
        }

        if (Uri.TryCreate(input, uriKind, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = uriKind switch
        {
            UriKind.Absolute => "Please enter a valid absolute URI (e.g., https://example.com)",
            UriKind.Relative => "Please enter a valid relative URI (e.g., /path/to/resource)",
            _ => "Please enter a valid URI",
        };
        value = null;
        return false;
    }
}
