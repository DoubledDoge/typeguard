using System.Net;
using System.Net.Sockets;

namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures an IP address is IPv4.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class Ipv4Rule(string? customMessage = null)
    : RulesBase<IPAddress>(
        v => v.AddressFamily == AddressFamily.InterNetwork,
        "IP address must be IPv4",
        customMessage
    );

/// <summary>
/// A validation rule that ensures an IP address is IPv6.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class Ipv6Rule(string? customMessage = null)
    : RulesBase<IPAddress>(
        v => v.AddressFamily == AddressFamily.InterNetworkV6,
        "IP address must be IPv6",
        customMessage
    );

/// <summary>
/// A validation rule that ensures an IP address is a private address.
/// Covers 10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16, and 169.254.0.0/16 (APIPA).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PrivateIpRule(string? customMessage = null) : IValidatorRule<IPAddress>
{
    /// <inheritdoc/>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork)
        {
            return false;
        }

        byte[] b = value.GetAddressBytes();

        bool is10Range = b[0] == 10;
        bool is172Range = b[0] == 172 && b[1] >= 16 && b[1] <= 31;
        bool is192Range = b[0] == 192 && b[1] == 168;
        bool isApipaRange = b[0] == 169 && b[1] == 254;

        return is10Range
            || is172Range
            || is192Range
            || isApipaRange;
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage
        ?? "IP address must be private (10.x.x.x, 172.16-31.x.x, 192.168.x.x, or 169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is an APIPA address (169.254.0.0/16).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ApipaRule(string? customMessage = null) : IValidatorRule<IPAddress>
{
    /// <inheritdoc/>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork)
        {
            return false;
        }

        byte[] b = value.GetAddressBytes();
        return b[0] == 169 && b[1] == 254;
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? "IP address must be an APIPA address (169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is not an APIPA address (169.254.0.0/16).
/// Useful for rejecting self-assigned addresses that indicate DHCP failure.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotApipaRule(string? customMessage = null) : IValidatorRule<IPAddress>
{
    /// <inheritdoc/>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != AddressFamily.InterNetwork)
        {
            return true;
        }

        byte[] b = value.GetAddressBytes();
        return !(b[0] == 169 && b[1] == 254);
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? "IP address cannot be an APIPA address (169.254.x.x)";
}

/// <summary>
/// A validation rule that ensures an IP address is a loopback address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LoopbackIpRule(string? customMessage = null)
    : RulesBase<IPAddress>(
        IPAddress.IsLoopback,
        "IP address must be a loopback address (127.0.0.1 or ::1)",
        customMessage
    );

/// <summary>
/// A validation rule that ensures an IP address is a public address.
/// Rejects private ranges, APIPA addresses, and loopback addresses.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PublicIpRule(string? customMessage = null) : IValidatorRule<IPAddress>
{
    /// <inheritdoc/>
    public bool IsValid(IPAddress value)
    {
        if (IPAddress.IsLoopback(value))
        {
            return false;
        }

        if (value.AddressFamily != AddressFamily.InterNetwork)
        {
            return true;
        }

        byte[] b = value.GetAddressBytes();

        return !(
            b[0] == 10
            || b[0] == 172 && b[1] >= 16 && b[1] <= 31
            || b[0] == 192 && b[1] == 168
            || b[0] == 169 && b[1] == 254
        );
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "IP address must be public";
}

/// <summary>
/// A validation rule that ensures an IP address is not a loopback address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotLoopbackIpRule(string? customMessage = null)
    : RulesBase<IPAddress>(
        v => !IPAddress.IsLoopback(v),
        "IP address cannot be a loopback address",
        customMessage
    );

/// <summary>
/// A validation rule that ensures an IP address falls within a specific subnet.
/// </summary>
/// <param name="network">The network address of the subnet. Cannot be null.</param>
/// <param name="prefixLength">
/// The subnet prefix length. Must be between 0 and 32 for IPv4, or 0 and 128 for IPv6.
/// </param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="network"/> is null.</exception>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="prefixLength"/> is invalid for the address family of <paramref name="network"/>.</exception>
public class SubnetRule(IPAddress network, int prefixLength, string? customMessage = null)
    : IValidatorRule<IPAddress>
{
    private readonly IPAddress _network = ValidateArgs(network, prefixLength);

    /// <inheritdoc/>
    public bool IsValid(IPAddress value)
    {
        if (value.AddressFamily != _network.AddressFamily)
        {
            return false;
        }

        byte[] addressBytes = value.GetAddressBytes();
        byte[] networkBytes = _network.GetAddressBytes();

        int fullBytes = prefixLength / 8;
        int remainingBits = prefixLength % 8;

        for (int i = 0; i < fullBytes; i++)
        {
            if (addressBytes[i] != networkBytes[i])
            {
                return false;
            }
        }

        if (remainingBits == 0)
        {
            return true;
        }

        byte mask = (byte)(0xFF << 8 - remainingBits);
        return (addressBytes[fullBytes] & mask) == (networkBytes[fullBytes] & mask);
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"IP address must be within subnet {network}/{prefixLength}";

    private static IPAddress ValidateArgs(IPAddress network, int prefixLength)
    {
        ArgumentNullException.ThrowIfNull(network);

        int maxPrefix = network.AddressFamily == AddressFamily.InterNetworkV6 ? 128 : 32;
        return prefixLength < 0 || prefixLength > maxPrefix
            ? throw new ArgumentOutOfRangeException(
                nameof(prefixLength),
                prefixLength,
                $"prefixLength must be between 0 and {maxPrefix} for {network.AddressFamily}."
            )
            : network;
    }
}

/// <summary>
/// A validation rule that ensures an IP address is in the specified collection of allowed addresses.
/// </summary>
/// <param name="allowedAddresses">The collection of allowed IP addresses. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedAddresses"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedAddresses"/> is empty.</exception>
public class AllowedIpAddressesRule(
    IEnumerable<IPAddress> allowedAddresses,
    string? customMessage = null
) : IValidatorRule<IPAddress>
{
    private readonly HashSet<IPAddress> _allowed = BuildSet(
        allowedAddresses,
        nameof(allowedAddresses)
    );

    /// <inheritdoc/>
    public bool IsValid(IPAddress value) => _allowed.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "IP address is not in the allowed list";

    private static HashSet<IPAddress> BuildSet(IEnumerable<IPAddress> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<IPAddress> set = [.. values];
        return set.Count == 0 ? throw new ArgumentException("Cannot be empty.", paramName) : set;
    }
}

/// <summary>
/// A validation rule that ensures an IP address is not in the specified collection of blocked addresses.
/// </summary>
/// <param name="blockedAddresses">The collection of blocked IP addresses. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="blockedAddresses"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="blockedAddresses"/> is empty.</exception>
public class BlockedIpAddressesRule(
    IEnumerable<IPAddress> blockedAddresses,
    string? customMessage = null
) : IValidatorRule<IPAddress>
{
    private readonly HashSet<IPAddress> _blocked = BuildSet(
        blockedAddresses,
        nameof(blockedAddresses)
    );

    /// <inheritdoc/>
    public bool IsValid(IPAddress value) => !_blocked.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "IP address is blocked";

    private static HashSet<IPAddress> BuildSet(IEnumerable<IPAddress> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<IPAddress> set = [.. values];
        return set.Count == 0 ? throw new ArgumentException("Cannot be empty.", paramName) : set;
    }
}
