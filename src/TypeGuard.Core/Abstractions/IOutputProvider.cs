namespace TypeGuard.Core.Abstractions;

public interface IOutputProvider
{
	Task DisplayPromptAsync(string message, CancellationToken cancellationToken = default);
	Task DisplayErrorAsync(string  message, CancellationToken cancellationToken = default);
	void DisplayPrompt(string      message);
	void DisplayError(string       message);
}