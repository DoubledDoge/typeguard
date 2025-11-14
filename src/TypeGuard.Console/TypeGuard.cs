namespace TypeGuard.Console;

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
}
