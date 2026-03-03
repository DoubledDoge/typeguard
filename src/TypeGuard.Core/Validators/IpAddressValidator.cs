using System.Net;

namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// A validator that prompts for and validates IP address input.
/// Supports both IPv4 and IPv6 addresses.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class IpAddressValidator(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<IPAddress>(inputProvider, outputProvider, prompt)
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid IPv4 or IPv6 address; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out IPAddress? value, out string? errorMessage)
    {
        if (IPAddress.TryParse(input, out value))
        {
            errorMessage = null;
            return true;
        }

        value = null;
        errorMessage = "Please enter a valid IP address (e.g., 192.168.1.1 or 2001:db8::1).";
        return false;
    }
}
