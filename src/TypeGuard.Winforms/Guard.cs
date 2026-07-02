using TypeGuard.Core;

namespace TypeGuard.WinForms;

/// <summary>
///     Provides a concise API for creating configured input builders for WinForms applications.
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
///         <see cref="Core.Builders.BuilderBase{T,TSelf}.Get" /> in WinForms. Calling <c>Get()</c>
///         from the UI thread will deadlock because it blocks synchronously over an async operation
///         while holding the synchronization context.
///     </para>
///     <example>
///         <code>
/// private readonly Guard _guard;
///
/// public MyForm()
/// {
///     InitializeComponent();
///     _guard = new Guard(inputTextBox, promptLabel, errorLabel);
/// }
///
/// private async void submitButton_Click(object sender, EventArgs e)
/// {
///     int age = await _guard.Int("Enter your age")
///         .WithRange(1, 120)
///         .GetAsync();
/// }
/// </code>
///     </example>
/// </remarks>
/// <param name="inputControl">The control to read user input from.</param>
/// <param name="promptLabel">The label used to display prompt messages.</param>
/// <param name="errorLabel">The label used to display error messages.</param>
public sealed class Guard(Control inputControl, Label promptLabel, Label errorLabel)
	: GuardBase<WinFormsInput, WinFormsOutput>(
		new WinFormsInput(inputControl),
		new WinFormsOutput(promptLabel, errorLabel)
	);
