namespace TypeGuard.Core.Validators;

using System.Net;
using Abstractions;

/// <summary>
/// A validator that prompts for and validates IP address input.
/// Supports both IPv4 and IPv6 addresses.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class IpAddressValidator(
    IInputProvider  inputProvider,
    IOutputProvider outputProvider,
    string          prompt
) : ValidatorBase<IPAddress>(inputProvider, outputProvider, prompt)
{
    /// <summary>
    /// Attempts to parse the raw user input into an IPAddress. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed IPAddress if parsing succeeded, or null if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid IP address; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out IPAddress? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = "Please enter a valid IP address";
            value = null;
            return false;
        }

        if (IPAddress.TryParse(input, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = "Please enter a valid IP address (e.g., 192.168.1.1 or 2001:db8::1)";
        value = null;
        return false;
    }
}
