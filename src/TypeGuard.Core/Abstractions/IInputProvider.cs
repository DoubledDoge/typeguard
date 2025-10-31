namespace TypeGuard.Core.Abstractions;

public interface IInputProvider
{
	Task<string?> GetInputAsync(CancellationToken cancellationToken = default);
	string?       GetInput(); // Sync wrapper
}

