using TypeGuard.Core.Interfaces;

namespace TypeGuard.Core.Handlers;

/// <summary>
/// An input handler that prompts for and validates URI input.
/// Accepts absolute or relative URIs depending on the specified kind.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.RelativeOrAbsolute"/>.</param>
public class UriHandler(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    UriKind uriKind = UriKind.RelativeOrAbsolute
) : HandlerBase<Uri>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="HandlerBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid URI of the specified <see cref="UriKind"/>; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out Uri? value, out string? errorMessage)
    {
        if (Uri.TryCreate(input, uriKind, out value))
        {
            errorMessage = null;
            return true;
        }

        value = null;
        errorMessage = uriKind switch
        {
            UriKind.Absolute => "Please enter a valid absolute URI (e.g., https://example.com).",
            UriKind.Relative => "Please enter a valid relative URI (e.g., /path/to/resource).",
            _ => "Please enter a valid URI.",
        };
        return false;
    }
}
