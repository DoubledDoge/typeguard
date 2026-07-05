namespace TypeGuard.Maui;

using Core;

/// <summary>
///     Provides a concise API for creating configured input builders for MAUI applications.
///     Supports Windows, macOS, iOS, and Android through MAUI's unified control model.
/// </summary>
/// <remarks>
///     Register this class in your MAUI DI container with the appropriate lifetime.
///     Use AddTransient for per-page validation or AddScoped for shared validation state
///     within a navigation scope:
///     builder.Services.AddTransient&lt;Guard&gt;()
///     <code>
/// _guard = new Guard(InputEntry, PromptLabel, ErrorLabel, SubmitButton);
/// int age = await _guard.Int("Enter your age")
///     .WithRange(1, 120)
///     .GetAsync();
/// </code>
/// </remarks>
/// <param name="entry">The entry control to read user input from.</param>
/// <param name="promptLabel">The label used to display prompt messages.</param>
/// <param name="errorLabel">The label used to display error messages.</param>
/// <param name="submitButton">
///     An optional button that the user taps to submit input. If null, input advances on the
///     keyboard return key instead. Recommended on mobile platforms.
/// </param>
public sealed class Guard(
	Entry entry,
	Label promptLabel,
	Label errorLabel,
	Button? submitButton = null
) : GuardBase<MauiInput, MauiOutput>(new(entry, submitButton), new(promptLabel, errorLabel));
