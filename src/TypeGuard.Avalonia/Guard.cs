using Avalonia.Controls;

namespace TypeGuard.Avalonia;

using Core;

/// <summary>
///     Provides a concise API for creating configured input builders for Avalonia applications.
/// </summary>
/// <remarks>
///     <para>
///         Each method returns a fluent builder that can be configured with validation rules via
///         <c>With*</c> methods before calling
///         <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync" /> to prompt the user and retrieve
///         a validated value.
///     </para>
///     <para>
///         Always prefer <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync" /> over
///         <see cref="Core.Builders.BuilderBase{T,TSelf}.Get" /> in Avalonia. Calling <c>Get()</c>
///         from the UI thread will deadlock because it blocks synchronously over an async operation
///         while holding the synchronization context.
///     </para>
///     <example>
///         <code>
/// private readonly Guard _guard;
///
/// public MyView()
/// {
///     InitializeComponent();
///     _guard = new Guard(InputTextBox, PromptBlock, ErrorBlock);
/// }
///
/// private async void SubmitButton_Click(object sender, RoutedEventArgs e)
/// {
///     int age = await _guard.Int("Enter your age")
///         .WithRange(1, 120)
///         .GetAsync();
/// }
/// </code>
///     </example>
/// </remarks>
/// <param name="inputTextBox">The text box to read user input from.</param>
/// <param name="promptBlock">The text block used to display prompt messages.</param>
/// <param name="errorBlock">The text block used to display error messages.</param>
public sealed class Guard(TextBox inputTextBox, TextBlock promptBlock, TextBlock errorBlock)
	: GuardBase<AvaloniaInput, AvaloniaOutput>(new(inputTextBox), new(promptBlock, errorBlock));
