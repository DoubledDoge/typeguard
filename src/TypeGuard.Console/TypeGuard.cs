namespace TypeGuard.Console;

using System.Net;
using System.Numerics;
using Core.Builders;
using Core.Validators;

/// <summary>
/// Provides convenient static methods for validating user input from the console with various different validation rules.
/// </summary>
public static class TypeGuard
{
    private static readonly ConsoleInput DefaultInputProvider = new();
    private static readonly ConsoleOutput DefaultOutputProvider = new();

    /// <summary>
    /// Creates a fluent builder for constructing a numeric validator with custom validation rules.
    /// Supports all numeric types including int, long, float, double, decimal, byte, short, uint, ulong, ushort, sbyte, nint, nuint, and Half.
    /// </summary>
    /// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="NumericBuilder{T}"/> instance for configuring validation rules.</returns>
    public static NumericBuilder<T> ForNumeric<T>(string prompt)
        where T : INumber<T>, IMinMaxValue<T> =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Creates a fluent builder for constructing an integer validator with integer-specific validation rules.
    /// Supports integer types including int, long, byte, short, uint, ulong, ushort, sbyte, nint, and nuint.
    /// </summary>
    /// <typeparam name="T">The integer type to validate. Must implement <see cref="IBinaryInteger{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>An <see cref="IntegerBuilder{T}"/> instance for configuring validation rules.</returns>
    public static IntegerBuilder<T> ForInteger<T>(string prompt)
        where T : IBinaryInteger<T>, IMinMaxValue<T> =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Asynchronously prompts the user for an int value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated int.</returns>
    public static async Task<int> GetIntAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<int>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a long value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated long.</returns>
    public static async Task<long> GetLongAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<long>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a short value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated short.</returns>
    public static async Task<short> GetShortAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<short>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a byte value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated byte.</returns>
    public static async Task<byte> GetByteAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<byte>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for an sbyte value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated sbyte.</returns>
    public static async Task<sbyte> GetSByteAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<sbyte>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a uint value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated uint.</returns>
    public static async Task<uint> GetUIntAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<uint>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a ulong value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated ulong.</returns>
    public static async Task<ulong> GetULongAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<ulong>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a ushort value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated ushort.</returns>
    public static async Task<ushort> GetUShortAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForInteger<ushort>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a double value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated double.</returns>
    public static async Task<double> GetDoubleAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForNumeric<double>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a float value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated float.</returns>
    public static async Task<float> GetFloatAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForNumeric<float>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a decimal value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated decimal.</returns>
    public static async Task<decimal> GetDecimalAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForNumeric<decimal>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Asynchronously prompts the user for a Half value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated Half.</returns>
    public static async Task<Half> GetHalfAsync(
        string prompt,
        CancellationToken cancellationToken = default
    ) => await ForNumeric<Half>(prompt).GetAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for an int value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated int.</returns>
    public static int GetInt(string prompt) => ForInteger<int>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a long value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated long.</returns>
    public static long GetLong(string prompt) => ForInteger<long>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a short value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated short.</returns>
    public static short GetShort(string prompt) => ForInteger<short>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a byte value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated byte.</returns>
    public static byte GetByte(string prompt) => ForInteger<byte>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for an sbyte value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated sbyte.</returns>
    public static sbyte GetSByte(string prompt) => ForInteger<sbyte>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a uint value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated uint.</returns>
    public static uint GetUInt(string prompt) => ForInteger<uint>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a ulong value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated ulong.</returns>
    public static ulong GetULong(string prompt) => ForInteger<ulong>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a ushort value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated ushort.</returns>
    public static ushort GetUShort(string prompt) => ForInteger<ushort>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a double value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated double.</returns>
    public static double GetDouble(string prompt) => ForNumeric<double>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a float value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated float.</returns>
    public static float GetFloat(string prompt) => ForNumeric<float>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a decimal value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated decimal.</returns>
    public static decimal GetDecimal(string prompt) => ForNumeric<decimal>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a Half value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated Half.</returns>
    public static Half GetHalf(string prompt) => ForNumeric<Half>(prompt).Get();

    /// <summary>
    /// Synchronously prompts the user for a string value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated non-empty string.</returns>
    public static string GetString(string prompt)
    {
        StringValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a string value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated non-empty string.</returns>
    public static async Task<string> GetStringAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        StringValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a string validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="StringBuilder"/> instance for configuring validation rules.</returns>
    public static StringBuilder ForString(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a DateTime value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
    /// <returns>The validated datetime in the specified format.</returns>
    public static DateTime GetDateTime(string prompt, string? format = null)
    {
        DateTimeValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt,
            format
        );
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a DateTime value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated datetime in the specified format.</returns>
    public static async Task<DateTime> GetDateTimeAsync(
        string prompt,
        string? format = null,
        CancellationToken cancellationToken = default
    )
    {
        DateTimeValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt,
            format
        );
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a DateTime validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date/time format string. If null, any valid DateTime format is accepted.</param>
    /// <returns>A <see cref="DateTimeBuilder"/> instance for configuring validation rules.</returns>
    public static DateTimeBuilder ForDateTime(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a GUID value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated GUID.</returns>
    public static Guid GetGuid(string prompt)
    {
        GuidValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a GUID value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated GUID.</returns>
    public static async Task<Guid> GetGuidAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        GuidValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a GUID validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="GuidBuilder"/> instance for configuring validation rules.</returns>
    public static GuidBuilder ForGuid(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a single character from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated character.</returns>
    public static char GetChar(string prompt)
    {
        CharValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a single character from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated character.</returns>
    public static async Task<char> GetCharAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        CharValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a character validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="CharBuilder"/> instance for configuring validation rules.</returns>
    public static CharBuilder ForChar(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a TimeSpan value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated TimeSpan.</returns>
    public static TimeSpan GetTimeSpan(string prompt)
    {
        TimeSpanValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a TimeSpan value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated TimeSpan.</returns>
    public static async Task<TimeSpan> GetTimeSpanAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        TimeSpanValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a TimeSpan validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">Optional format hint for parsing. If null, accepts any valid TimeSpan format.</param>
    /// <returns>A <see cref="TimeSpanBuilder"/> instance for configuring validation rules.</returns>
    public static TimeSpanBuilder ForTimeSpan(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a Uri value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated Uri.</returns>
    public static Uri GetUri(string prompt)
    {
        UriValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a Uri value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated Uri.</returns>
    public static async Task<Uri> GetUriAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        UriValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a Uri validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="uriKind">The type of URI to accept. Default is Absolute.</param>
    /// <returns>A <see cref="UriBuilder"/> instance for configuring validation rules.</returns>
    public static UriBuilder ForUri(string prompt, UriKind uriKind = UriKind.Absolute) =>
        new(prompt, uriKind, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for an enum value from the console.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Default is true.</param>
    /// <returns>The validated enum.</returns>
    public static TEnum GetEnum<TEnum>(string prompt, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        EnumValidator<TEnum> validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt,
            ignoreCase
        );
        return validator.GetValidInput();
    }

    ///  <summary>
    ///  Synchronously prompts the user for an enum value from the console.
    ///  </summary>
    ///  <typeparam name="TEnum">The enum type to validate.</typeparam>
    ///  <param name="prompt">The prompt message to display to the user.</param>
    ///  <param name="ignoreCase">If true, enum name parsing is case-insensitive. Default is true.</param>
    ///  <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    ///  <returns>The validated enum.</returns>
    public static async Task<TEnum> GetEnumAsync<TEnum>(
        string prompt,
        bool ignoreCase = true,
        CancellationToken cancellationToken = default
    )
        where TEnum : struct, Enum
    {
        EnumValidator<TEnum> validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt,
            ignoreCase
        );
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing an enum validator with custom validation rules.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate.</typeparam>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Default is true.</param>
    /// <returns>An <see cref="EnumBuilder{TEnum}"/> instance for configuring validation rules.</returns>
    public static EnumBuilder<TEnum> ForEnum<TEnum>(string prompt, bool ignoreCase = true)
        where TEnum : struct, Enum =>
        new(prompt, ignoreCase, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a DateOnly value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated DateOnly.</returns>
    public static DateOnly GetDateOnly(string prompt)
    {
        DateOnlyValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a DateOnly value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated DateOnly.</returns>
    public static async Task<DateOnly> GetDateOnlyAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        DateOnlyValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a DateOnly validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected date format string. If null, any valid DateOnly format is accepted.</param>
    /// <returns>A <see cref="DateOnlyBuilder"/> instance for configuring validation rules.</returns>
    public static DateOnlyBuilder ForDateOnly(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for a TimeOnly value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated TimeOnly.</returns>
    public static TimeOnly GetTimeOnly(string prompt)
    {
        TimeOnlyValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for a TimeOnly value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated TimeOnly.</returns>
    public static async Task<TimeOnly> GetTimeOnlyAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        TimeOnlyValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing a TimeOnly validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="format">The expected time format string. If null, any valid TimeOnly format is accepted.</param>
    /// <returns>A <see cref="TimeOnlyBuilder"/> instance for configuring validation rules.</returns>
    public static TimeOnlyBuilder ForTimeOnly(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);

    /// <summary>
    /// Synchronously prompts the user for an IPAddress value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>The validated IPAddress.</returns>
    public static IPAddress GetIpAddress(string prompt)
    {
        IpAddressValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return validator.GetValidInput();
    }

    /// <summary>
    /// Asynchronously prompts the user for an IPAddress value from the console.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated IPAddress.</returns>
    public static async Task<IPAddress> GetIpAddressAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        IpAddressValidator validator = new(DefaultInputProvider, DefaultOutputProvider, prompt);
        return await validator.GetValidInputAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a fluent builder for constructing an IP address validator with custom validation rules.
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user.</param>
    /// <returns>A <see cref="IpAddressBuilder"/> instance for configuring validation rules.</returns>
    public static IpAddressBuilder ForIpAddress(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);
}
