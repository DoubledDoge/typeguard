using System.Numerics;

namespace TypeGuard.Core.Validators;

using Interfaces;

/// <summary>
/// A validator that prompts for and validates numeric input of any type that implements <see cref="INumber{TSelf}"/>.
/// Supports all numeric types including int, long, float, double, decimal, byte, short, uint, ulong, ushort, sbyte, and Half.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class NumericValidator<T>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt
) : ValidatorBase<T>(inputProvider, outputProvider, prompt)
    where T : INumber<T>
{
    /// <inheritdoc cref="ValidatorBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid <typeparamref name="T"/> value; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out T? value, out string? errorMessage)
    {
        if (T.TryParse(input, null, out value))
        {
            errorMessage = null;
            return true;
        }

        value = default;
        errorMessage = $"Please enter a valid {typeof(T).Name}.";
        return false;
    }
}
