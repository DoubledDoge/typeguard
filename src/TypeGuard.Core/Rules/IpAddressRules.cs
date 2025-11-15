namespace TypeGuard.Core.Rules;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// A validation rule that ensures an IP address is IPv4.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class Ipv4Rule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is IPv4.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is IPv4; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => value.AddressFamily == AddressFamily.InterNetwork;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be IPv4";
}

/// <summary>
/// A validation rule that ensures an IP address is IPv6.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class Ipv6Rule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is IPv6.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is IPv6; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => value.AddressFamily == AddressFamily.InterNetworkV6;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be IPv6";
}

/// <summary>
/// A validation rule that ensures an IP address is a private address.
/// Includes standard private ranges and APIPA addresses.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class PrivateIpRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is private.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is private; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork) return false;
        byte[] bytes = value.GetAddressBytes();
        switch (bytes[0])
        {
            case 10:                                        // 10.0.0.0/8
            case 172 when bytes[1] >= 16 && bytes[1] <= 31: // 172.16.0.0/12
            case 192 when bytes[1] == 168:                  // 192.168.0.0/16
            case 169 when bytes[1] == 254:                  // 169.254.0.0/16 (APIPA)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be private (10.x.x.x, 172.16-31.x.x, 192.168.x.x, or 169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is an APIPA address.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class ApipaRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is an APIPA address.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is in the APIPA range (169.254.0.0 - 169.254.255.255); otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork) return false;

        byte[] bytes = value.GetAddressBytes();

        return bytes[0] == 169 && bytes[1] == 254;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be an APIPA address (169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is not an APIPA address.
/// Useful for rejecting self-assigned addresses that indicate DHCP failure.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class NotApipaRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is not an APIPA address.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is not in the APIPA range; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork) return true;

        byte[] bytes = value.GetAddressBytes();

        return !(bytes[0] == 169 && bytes[1] == 254);
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address cannot be APIPA (169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is a loopback address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LoopbackIpRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is a loopback address.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is loopback; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => IPAddress.IsLoopback(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be loopback (127.0.0.1 or ::1)";
}

/// <summary>
/// A validation rule that ensures an IP address is a public address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PublicIpRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    private readonly PrivateIpRule _privateCheck = new();
    private readonly LoopbackIpRule _loopbackCheck = new();

    /// <summary>
    /// Determines whether the specified IP address is public.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is public; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => !_privateCheck.IsValid(value) && !_loopbackCheck.IsValid(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address must be public";
}

/// <summary>
/// A validation rule that ensures an IP address is not a loopback address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotLoopbackIpRule(string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is not a loopback address.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is not loopback; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => !IPAddress.IsLoopback(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address cannot be loopback";
}

/// <summary>
/// A validation rule that ensures an IP address is within a specific subnet.
/// </summary>
/// <param name="network">The network address.</param>
/// <param name="prefixLength">The subnet prefix length.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class SubnetRule(IPAddress network, int prefixLength, string? customMessage = null) : IValidationRule<IPAddress>
{
    /// <summary>
    /// Determines whether the specified IP address is within the subnet.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is within the subnet; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != network.AddressFamily)
            return false;

        byte[] addressBytes = value.GetAddressBytes();
        byte[] networkBytes = network.GetAddressBytes();

        int bytesToCheck = prefixLength / 8;
        int bitsToCheck = prefixLength % 8;

        // Check full bytes
        for (int i = 0; i < bytesToCheck; i++)
        {
            if (addressBytes[i] != networkBytes[i]) // Compare byte by byte
                return false;
        }

        // Check remaining bits
        if (bitsToCheck <= 0) return true;

        byte mask = (byte)(0xFF << (8 - bitsToCheck)); // Create a mask for the remaining bits

        return (addressBytes[bytesToCheck] & mask) == (networkBytes[bytesToCheck] & mask); // Compare only the relevant bits
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"IP address must be within subnet {network}/{prefixLength}";
}

/// <summary>
/// A validation rule that ensures an IP address is in a list of allowed addresses.
/// </summary>
/// <param name="allowedAddresses">The collection of allowed IP addresses.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class AllowedIpAddressesRule(IEnumerable<IPAddress> allowedAddresses, string? customMessage = null) : IValidationRule<IPAddress>
{
    private readonly HashSet<IPAddress> _allowed = [..allowedAddresses];

    /// <summary>
    /// Determines whether the specified IP address is in the allowed list.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is allowed; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => _allowed.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address is not in the allowed list";
}

/// <summary>
/// A validation rule that ensures an IP address is not in a list of blocked addresses.
/// </summary>
/// <param name="blockedAddresses">The collection of blocked IP addresses.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class BlockedIpAddressesRule(IEnumerable<IPAddress> blockedAddresses, string? customMessage = null) : IValidationRule<IPAddress>
{
    private readonly HashSet<IPAddress> _blocked = [..blockedAddresses];

    /// <summary>
    /// Determines whether the specified IP address is not in the blocked list.
    /// </summary>
    /// <param name="value">The IP address to validate.</param>
    /// <returns><c>true</c> if the IP address is not blocked; otherwise, <c>false</c>.</returns>
    public bool IsValid(IPAddress value) => !_blocked.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "IP address is blocked";
}
