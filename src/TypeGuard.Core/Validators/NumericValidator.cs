namespace TypeGuard.Core.Validators;

using System.Numerics;
using Abstractions;

/// <summary>
/// A validator that prompts for and validates numeric input of any type that implements <see cref="INumber{TSelf}"/>.
/// Supports all numeric types including: int, long, float, double, decimal, byte, short, uint, ulong, ushort, sbyte, and Half
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class NumericValidator<T>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<T>(inputProvider, outputProvider, prompt)
    where T : INumber<T>, IMinMaxValue<T>
{
    /// <summary>
    /// Attempts to parse the raw user input into a numeric value of type <typeparamref name="T"/>. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed numeric value if parsing succeeded, or the default value if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid numeric value of type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out T? value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = $"Please enter a valid {typeof(T).Name}";
            value = default(T);
            return false;
        }

        if (T.TryParse(input, null, out T? result))
        {
            value = result;
            errorMessage = null;
            return true;
        }

        errorMessage = $"Please enter a valid {typeof(T).Name}";
        value = default(T);
        return false;
    }
}
