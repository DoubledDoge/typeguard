using System.Runtime.CompilerServices;

namespace TypeGuard.Core.Rules;

internal static class BuildHelper<T>
{
	internal static HashSet<T> BuildSet(
		IEnumerable<T> values,
		[CallerArgumentExpression(nameof(values))] string? paramName = null
	)
	{
		ArgumentNullException.ThrowIfNull(values, paramName);
		HashSet<T> set = [.. values];
		return set.Count == 0 ? throw new ArgumentException("Cannot be empty.", paramName) : set;
	}
}
